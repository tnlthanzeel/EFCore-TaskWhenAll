using AutoMapper;
using GemSto.Common;
using GemSto.Common.Constants;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Export;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace GemSto.Service
{
    public class ExportService : IExportService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;
        private readonly IGemService gemService;

        public ExportService(GemStoContext gemStoContext, IMapper mapper, IGemService gemService)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
            this.gemService = gemService;
        }
        public async Task<string> CreateExport(ExportCreateModel exportCreateModel)
        {
            try
            {
                var entity = mapper.Map<Export>(exportCreateModel);

                await gemStoContext.Exports.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
                return entity.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while creating export, try again");
            }
        }

        public async Task<PaginationModel<ExportSummaryModel>> GetAllExports(PaginationBase paginationBase)
        {
            var query = gemStoContext.Exports.Where(w => !w.IsDeleted);

            if (paginationBase.SearchQuery != null)
            {
                query = query.Where(x => EF.Functions.Like(x.ExportNumber.ToString(), paginationBase.SearchQuery + "%") ||
                 EF.Functions.Like(x.Origin.Value, paginationBase.SearchQuery + "%") ||
                EF.Functions.Like(x.Description, "%" + paginationBase.SearchQuery + "%")
                );
            }

            var totalNumberOfRecord = query.Count();

            query = query.OrderByDescending(x => x.ExportDate).Skip(paginationBase.Skip).Take(paginationBase.Take);

            var result = query.Select(x => new ExportSummaryModel
            {
                Id = x.Id.ToString(),
                CountryName = x.Origin.Value,
                Date = x.ExportDate,
                Description = x.Description,
                NumberOfPieces = x.GemExports.Count(c => !c.IsDeleted),
                ExportNumber = x.ExportNumber,
                IsExportClosed = x.IsExportClosed
            }).AsQueryable();

            var resultData = await result.AsNoTracking().ToListAsync();

            var resultSet = new PaginationModel<ExportSummaryModel>()
            {
                TotalRecords = totalNumberOfRecord,
                Details = resultData
            };
            return resultSet;
        }

        public Task GetExportById(string exportId)
        {
            throw new NotImplementedException();
        }

        public Task GetExportDetailById(Guid exportId)
        {
            throw new NotImplementedException();
        }

        public async Task<ExportCreateModel> GetExportHeaderById(string exportId)
        {
            var entity = await gemStoContext.Exports.Include(x => x.Origin).FirstAsync(f => f.Id == Guid.Parse(exportId));
            var model = mapper.Map<ExportCreateModel>(entity);
            return model;
        }

        public async Task<int> GetLastExprtIdAsync()
        {
            var data = await gemStoContext.Exports.OrderBy(o => o.ExportNumber).LastOrDefaultAsync();
            return (data == null ? 1 : ++data.ExportNumber);
        }

        public async Task UpdateExportSummaryAync(ExportCreateModel exportCreateModel)
        {
            var entity = await gemStoContext.Exports.FindAsync(exportCreateModel.Id);
            entity.OriginId = exportCreateModel.OriginId;
            entity.Description = exportCreateModel.Description;
            entity.ExportDate = exportCreateModel.ExportDate;

            gemStoContext.Entry(entity).State = EntityState.Modified;
            await gemStoContext.SaveChangesAsync();
        }

        public async Task AddGemToExportAsync(GemToExport gemToExport)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();
                try
                {
                    var lastExportGem = await gemStoContext.GemExports.Include(i => i.Export).OrderBy(o => o.Number)
                        .LastOrDefaultAsync(l => l.ExportId == gemToExport.ExportId && !l.IsDeleted);

                    var entity = mapper.Map<GemExport>(gemToExport);
                    entity.Number = lastExportGem == null ? 1 : lastExportGem.Number + 1;

                    gemStoContext.GemExports.Add(entity);
                    await gemStoContext.SaveChangesAsync();


                    var gemEntity = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == gemToExport.GemId);
                    var gemStatusBeforeExport = gemEntity.GemStatus;

                    gemEntity.GemStatus = GemStatus.Exported;

                    var export = await gemStoContext.Exports.FirstOrDefaultAsync(w => w.Id == gemToExport.ExportId);

                    gemHistory.Add(new GemHistory().CreateExportHistory(gemToExport.CreatedById, GemHistoryStatusEnum.AddedToExport, entity.Id, $"Added to export for Export Id:- {export.ExportNumber}", gemEntity.Id, ActionEnum.Created, gemToExport.CreatedByName, gemStatusBeforeExport));

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw new Exception("Something went wrong while adding gem to export, try again");
                }
            }
        }

        public async Task<PaginationModel<ExportDetailModel>> GetExportDetailByIdAsync(PaginationBase paginationBase, Guid exportId)
        {
            try
            {

                var query = gemStoContext.GemExports.Where(w => w.ExportId == exportId && !w.IsDeleted);

                if (!string.IsNullOrEmpty(paginationBase.OrderBy))
                {
                    query = query.OrderBy(paginationBase.OrderBy);
                }

                if (!string.IsNullOrEmpty(paginationBase.SearchQuery))
                {
                    paginationBase.SearchQuery = paginationBase.SearchQuery.ToLower();
                    query = query.OrderByDescending(o => o.CreatedOn);

                    query = query.Where(w => !w.IsDeleted &&
                    (
                        EF.Functions.Like(w.Number.ToString(), paginationBase.SearchQuery) ||
                        EF.Functions.Like(w.Gem.StockNumber, paginationBase.SearchQuery + "%") ||
                        EF.Functions.Like(w.Owner.ToLower(), paginationBase.SearchQuery + "%") ||

                        w.Gem.Certificates.Any(a => a.IsDefault && EF.Functions.Like(a.CertificateProvider.Description, paginationBase.SearchQuery + "%")) ||
                        w.Gem.Certificates.Any(a => a.IsDefault && EF.Functions.Like(a.CertificateProvider.Value, paginationBase.SearchQuery + "%")) ||

                        w.Gem.Certificates.Any(a => a.IsDefault && EF.Functions.Like(a.Origin.Value, paginationBase.SearchQuery + "%")) ||
                        w.Gem.Certificates.Any(a => EF.Functions.Like(a.Origin.Description, paginationBase.SearchQuery + "%")) ||

                        w.Gem.Certificates.Any(a => a.IsDefault && EF.Functions.Like(a.Colour.Value, paginationBase.SearchQuery + "%")) ||
                        w.Gem.Certificates.Any(a => a.IsDefault && EF.Functions.Like(a.Colour.Description, paginationBase.SearchQuery + "%")) ||

                        EF.Functions.Like(w.CertificateProvider.Value, paginationBase.SearchQuery + "%") ||
                        EF.Functions.Like(w.Colour.Value, paginationBase.SearchQuery + "%") ||
                        EF.Functions.Like(w.Colour.Description, paginationBase.SearchQuery + "%") ||
                        EF.Functions.Like(w.Origin.Value, paginationBase.SearchQuery + "%") ||

                        EF.Functions.Like(w.Weight.Value.ToString(), "%" + paginationBase.SearchQuery + "%") ||

                        (w.Gem.RecutWeight != 0.00m ? EF.Functions.Like(w.Gem.RecutWeight.ToString(), "%" + paginationBase.SearchQuery + "%") : EF.Functions.Like(w.Gem.InitialWeight.ToString(), "%" + paginationBase.SearchQuery + "%"))
                    )
                  );
                }

                var totalRecords = await query.CountAsync();
                var totalSold = await query.CountAsync(c => c.GemStatus == GemStatus.Sold || c.Gem.GemStatus == GemStatus.SoldCP || c.Gem.GemStatus == GemStatus.Sold);
                var isExportClosed = (await gemStoContext.Exports.FirstOrDefaultAsync(f => f.Id == exportId)).IsExportClosed;

                query = query.Skip(paginationBase.Skip).Take(paginationBase.Take);

                var result = query.Select(s => new ExportDetailModel
                {
                    GemExportId = s.Id,
                    variety = s.GemId != null ? s.Gem.Variety.Value : s.Variety.Value,
                    Weight = s.GemId != null ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : (s.Weight.Value),
                    Owner = s.GemId != null ? s.Gem.StockNumber : s.Owner,
                    Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,

                    Certificate = s.GemId != null ? s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).CertificateProvider.Value + " - " +
                    s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Colour.Value +
                     (s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Origin.Value != null ? " - " + s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Origin.Value : string.Empty) :
                     s.CertificateProvider.Value + " - " + s.Colour.Value + (s.OriginId != null ? " - " + s.Origin.Value : string.Empty),

                    Cost = s.GemId != null ? s.Gem.TotalCost.ToString() : s.Cost,
                    IsTreated = s.GemId != null ? s.Gem.IsTreated.Value : s.IsTreated.Value,
                    GemStatus = s.GemId != null ? s.Gem.GemStatus : s.GemStatus.Value,
                    IsThirdParty = s.IsThirdParty,
                    Number = s.Number,
                    GemId = s.GemId.HasValue ? s.GemId : (int?)null,
                    GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty
                }).AsQueryable();

                var resultModel = await result.AsNoTracking().ToListAsync();

                var resultSet = new PaginationModel<ExportDetailModel>
                {
                    Details = resultModel,
                    TotalRecords = totalRecords,
                    ExtensionData = new { isExportClosed, totalSold }
                };

                return resultSet;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddThirdPartyGemToExportAsync(ExportThirdPartyCreateModel exportThirdPartyCreateModel)
        {
            try
            {
                var lastExportGem = await gemStoContext.GemExports.OrderBy(o => o.Number)
                    .LastOrDefaultAsync(l => l.ExportId == exportThirdPartyCreateModel.ExportId && !l.IsDeleted);

                var entity = mapper.Map<GemExport>(exportThirdPartyCreateModel);

                entity.Number = lastExportGem == null ? 1 : lastExportGem.Number + 1;
                entity.IsThirdParty = true;
                await gemStoContext.GemExports.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RemoveGemFromExportAsync(int exportId, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();

                try
                {
                    var entity = await gemStoContext.GemExports.Include(i => i.Export).Include(i => i.Gem).FirstOrDefaultAsync(f => !f.IsDeleted && f.Id == exportId);


                    if (entity.GemId != null)
                    {
                        gemHistory.Add(new GemHistory().CreateExportHistory(editedById, GemHistoryStatusEnum.RemovedFromExport, entity.Id, $"Removed from export for Export Id:- {entity.Export.ExportNumber}", (int)entity.GemId, ActionEnum.Removed, editedByName, entity.Gem.GemStatus));

                        gemStoContext.GemHistory.AddRange(gemHistory);

                        entity.Gem.GemStatus = GemStatus.InStock;
                    }

                    else
                    {
                        entity.GemStatus = GemStatus.Returned;
                    }

                    entity.IsDeleted = true;
                    entity.EditedById = editedById;
                    entity.EditedOn = DateTimeOffset.UtcNow;

                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<ExportDetailModel> GetExportDetailByIdAsync(int gemExportId)
        {
            var query = gemStoContext.GemExports.Where(w => w.Id == gemExportId && !w.IsDeleted);


            var result = await query.Select(s => new ExportDetailModel
            {
                GemExportId = s.Id,
                VarietyId = s.VarietyId,
                Weight = s.Weight.Value,
                Owner = s.Owner,
                ShapeId = s.ShapeId,
                CertificateProviderId = s.CertificateProviderId,
                ColourId = s.ColourId,
                OriginId = s.OriginId,
                Cost = s.GemId != null ? s.Gem.TotalCost.ToString() : s.Cost,
                IsTreated = s.GemId != null ? s.Gem.IsTreated.Value : s.IsTreated.Value,
                ExportId = s.ExportId
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task UpdateThridPartyGemAsync(ThirdPartyUpdateModel thirdPartyUpdateModel)
        {
            var entity = await gemStoContext.GemExports.FirstOrDefaultAsync(f => f.Id == thirdPartyUpdateModel.Id && f.ExportId == thirdPartyUpdateModel.ExportId && !f.IsDeleted);

            entity.OriginId = thirdPartyUpdateModel.OriginId;
            entity.Owner = thirdPartyUpdateModel.Owner;
            entity.ShapeId = thirdPartyUpdateModel.ShapeId;
            entity.VarietyId = thirdPartyUpdateModel.VarietyId;
            entity.Weight = thirdPartyUpdateModel.Weight;
            entity.ColourId = thirdPartyUpdateModel.ColourId;
            entity.Cost = thirdPartyUpdateModel.Cost;
            entity.IsTreated = thirdPartyUpdateModel.IsTreated;
            entity.CertificateProviderId = thirdPartyUpdateModel.CertificateProviderId;
            entity.EditedOn = DateTimeOffset.UtcNow;

            gemStoContext.Entry(entity).State = EntityState.Modified;
            await gemStoContext.SaveChangesAsync();
        }

        public async Task ChangeStockIdForExportAsync(int id, int gemIdToUpdate, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();

                try
                {

                    var oldEntity = await gemStoContext.GemExports.Include(i => i.Export).Include(i => i.Gem).FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

                    gemHistory.Add(new GemHistory().CreateExportHistory(editedById, GemHistoryStatusEnum.RemovedFromExport, oldEntity.Id, $"Removed from export for Export Id:- {oldEntity.Export.ExportNumber}", (int)oldEntity.GemId, ActionEnum.Edited, editedByName, oldEntity.Gem.GemStatus));


                    //change existig gem status to in-stock
                    oldEntity.Gem.GemStatus = GemStatus.InStock;

                    //set new gem id to export
                    oldEntity.GemId = gemIdToUpdate;

                    var newGemToExport = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == gemIdToUpdate && !f.IsDeleted);

                    gemHistory.Add(new GemHistory().CreateExportHistory(editedById, GemHistoryStatusEnum.AddedToExport, oldEntity.Id, $"Added to export for Export Id:- {oldEntity.Export.ExportNumber}", gemIdToUpdate, ActionEnum.Edited, editedByName, newGemToExport.GemStatus));

                    newGemToExport.GemStatus = GemStatus.Exported;


                    oldEntity.EditedById = editedById;
                    oldEntity.Gem.EditedOn = oldEntity.EditedOn = DateTimeOffset.UtcNow;

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task CloseExportAsync(Guid exportId, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();

                try
                {
                    var gemsRemovedFromExport = await gemStoContext.GemExports.Include(i => i.Export).Include(i => i.Gem).Where(w => w.ExportId == exportId && w.Gem.GemStatus == GemStatus.Exported && !w.IsDeleted && w.GemId.HasValue).ToListAsync();

                    foreach (var gemExport in gemsRemovedFromExport)
                    {
                        gemHistory.Add(new GemHistory().CreateExportHistory(editedById, GemHistoryStatusEnum.RemovedFromExport, gemExport.Id, $"Removed from export for Export Id:- {gemExport.Export.ExportNumber}", (int)gemExport.GemId, ActionEnum.Removed, editedByName, gemExport.Gem.GemStatus));
                    }

                    var exportIdAsString = exportId.ToString();
                    var result = await gemStoContext.Database.ExecuteSqlCommandAsync("[dbo].[spCloseExport] @p0", parameters: exportIdAsString);

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteExport(Guid exportId, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();

                try
                {

                    var result = await gemStoContext.Exports.Include(i => i.GemExports).ThenInclude(i => i.Gem).FirstOrDefaultAsync(f => f.Id == exportId && !f.IsDeleted);
                    result.IsDeleted = true;
                    result.IsExportClosed = true;

                    var removeFromExport = result.GemExports.Where(w => w.GemId != null && !w.IsThirdParty).ToList()
                         .Where(w => w.Gem.GemStatus == GemStatus.Exported).ToList();


                    foreach (var gemExport in removeFromExport)
                    {
                        gemHistory.Add(new GemHistory().CreateExportHistory(editedById, GemHistoryStatusEnum.RemovedFromExport, gemExport.Id, $"Removed from export for Export Id:- {gemExport.Export.ExportNumber} on deleting Export", (int)gemExport.GemId, ActionEnum.Deleted, editedByName, gemExport.Gem.GemStatus));

                        gemExport.Gem.GemStatus = GemStatus.InStock;
                    }

                    result.EditedById = editedById;
                    result.EditedOn = DateTimeOffset.Now;

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }
    }
}

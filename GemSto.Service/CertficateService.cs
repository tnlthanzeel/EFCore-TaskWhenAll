using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using GemSto.Service.Models.Certification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class CertficateService : ICertificateService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public CertficateService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<bool> CreateAsync(CertificateCreateModel certificateCreateModel, decimal? length = null, decimal? width = null, decimal? depth = null, int? recutShapeId = null, decimal? recutWeight = null)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemHistoryList = new List<GemHistory>();

                    var isAny = gemStoContext.Certificates.Any(a => a.Number == certificateCreateModel.Number &&
                    a.CertificateProviderId == certificateCreateModel.CertificateProviderId);

                    if (isAny)
                    {
                        return false;
                    }

                    var single = await gemStoContext.Gems.FirstOrDefaultAsync(f => !f.IsDeleted && f.Id == certificateCreateModel.GemId);
                    var gemStatusBeforeAddingCertificate = single.GemStatus;



                    var transaction = new Transaction
                    {
                        PaidAmount = certificateCreateModel.CertificateFee,
                        GemId = certificateCreateModel.GemId,
                        GemStatus = single.GemStatus,
                        PaidOn = DateTimeOffset.UtcNow,
                        Remark = Remark.Created,
                        TransactionType = TransactionType.CertificatePayment,
                    };

                    if (single.IsGemLot)
                    {
                        var gemLotHeaderStockNumber = single.StockNumber.Split("-")[0];
                        var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.StockNumber == gemLotHeaderStockNumber);

                        gemLotHeader.TotalCost += certificateCreateModel.CertificateFee;

                        gemStoContext.Entry(gemLotHeader).State = EntityState.Modified;
                    }

                    gemStoContext.Transactions.Add(transaction);

                    var certificatEntity = mapper.Map<Certificate>(certificateCreateModel);
                    certificatEntity.TransactionId = transaction.Id;

                    var priorCertificates = await gemStoContext.Certificates.Where(w => w.GemId == certificateCreateModel.GemId && !w.IsDeleted).ToListAsync();

                    if (priorCertificates.Count() != 0)
                    {
                        priorCertificates.ForEach(x => x.IsDefault = false);
                    }


                    if (length.HasValue && width.HasValue && depth.HasValue)
                    {
                        single.Length = (decimal)length;
                        single.Width = (decimal)width;
                        single.Depth = (decimal)depth;
                    }

                    if (recutShapeId != null)
                    {
                        single.RecutShapeId = recutShapeId;
                    }

                    if (recutWeight != null && recutWeight > 0.00m)
                    {
                        single.RecutWeight = (decimal)recutWeight;
                    }

                    certificatEntity.IsDefault = true;
                    single.TotalCost += certificateCreateModel.CertificateFee;
                    gemStoContext.Certificates.Add(certificatEntity);

                    await gemStoContext.SaveChangesAsync();

                    var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificateCreateModel.CertificateProviderId && !w.IsDeleted);

                    var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                    gemHistoryList.Add(new GemHistory().CreateCertificateHistory(certificateCreateModel.CreatedById, GemHistoryStatusEnum.Certified, certificatEntity.Id, $"Added new certificate provided by {certProvidedBy}", single.Id, ActionEnum.Created, certificateCreateModel.CreatedByName, gemStatusBeforeAddingCertificate));

                    gemStoContext.GemHistory.AddRange(gemHistoryList);

                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(int id, string editedById, string editedByName)
        {
            var gemHistoryList = new List<GemHistory>();

            var entity = await gemStoContext.Gems
                .Include(i => i.Certificates)
                .ThenInclude(i => i.Transaction)
                .FirstOrDefaultAsync(f => !f.IsDeleted && f.Certificates.Any(a => a.Id == id));

            var certificateEntity = entity.Certificates.FirstOrDefault(f => f.Id == id && !f.IsDeleted);
            var certificateTransaction = entity.Transactions
                .FirstOrDefault(f => f.TransactionType == TransactionType.CertificatePayment && f.Id == certificateEntity.TransactionId);
            entity.TotalCost += -certificateTransaction.PaidAmount;
            certificateTransaction.Remark = Remark.Deleted;
            certificateTransaction.EditedOn = DateTimeOffset.UtcNow;
            certificateEntity.IsDeleted = true;

            if (entity.IsGemLot)
            {
                var gemLotHeaderStockNumber = entity.StockNumber.Split("-")[0];
                var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.StockNumber == gemLotHeaderStockNumber);

                gemLotHeader.TotalCost += -certificateTransaction.PaidAmount;

                gemStoContext.Entry(gemLotHeader).State = EntityState.Modified;
            }

            var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificateEntity.CertificateProviderId);

            var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

            gemHistoryList.Add(new GemHistory().CreateCertificateHistory(editedById, GemHistoryStatusEnum.RemovedCertificate, certificateEntity.Id, $"Deleted certificate provided by {certProvidedBy}", entity.Id, ActionEnum.Removed, editedByName, entity.GemStatus));

            gemStoContext.Entry(entity).State = EntityState.Modified;
            gemStoContext.GemHistory.AddRange(gemHistoryList);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<List<CertificateModel>> GetCertificatesByGemId(int id)
        {
            try
            {
                var query = gemStoContext.Certificates.Where(w => w.GemId == id && !w.IsDeleted);

                var model = await query.Select(s => new CertificateModel
                {
                    Id = s.Id,
                    CerticateProvider = s.CertificateProvider.Value,
                    CertProviderAgent = s.CertificateProvider.Agent ?? "-",
                    CerticateNumber = s.Number,
                    Color = new ColourModel { Value = s.Colour.Value, Description = s.Colour.Description },
                    CertificateFee = s.Transaction.PaidAmount,
                    IsTreated = s.IsTreated,
                    OriginName = s.Origin.Value,
                    IsDefault = s.IsDefault,
                    CertifiedDate = s.CertifiedDate,
                    Description = s.Description,
                    CertURL = s.CertURL
                }).AsNoTracking().ToListAsync();

                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<CertificateModel> GetByIdAsync(int certificateId)
        {
            try
            {
                var model = await gemStoContext.Certificates.Where(c => c.Id == certificateId && !c.IsDeleted).Select(s => new CertificateModel()
                {
                    CertificateFee = s.Transaction.PaidAmount,
                    CerticateNumber = s.Number,
                    CertificateproviderId = s.CertificateProviderId,
                    ColourId = s.ColourId,
                    Id = s.Id,
                    IsTreated = s.IsTreated,
                    OriginId = s.OriginId,
                    Description = s.Description,
                    CertifiedDate = s.CertifiedDate,
                    CertURL = s.CertURL
                }).AsNoTracking().FirstOrDefaultAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCertificateDetailAsync(CertificateUpdateModel certificateUpdateModel)
        {
            try
            {
                var gemHistoryList = new List<GemHistory>();


                var isCertificateNumberTaken = await gemStoContext.Certificates
                    .AnyAsync(f => f.Number == certificateUpdateModel.CerticateNumber && !f.IsDeleted && f.Id != certificateUpdateModel.Id);

                if (isCertificateNumberTaken)
                {
                    throw new Exception("Certificate number is already in use");
                }

                var entity = await gemStoContext.Certificates.Include(i => i.Gem).Include(i => i.Transaction)
                     .FirstOrDefaultAsync(f => f.Id == certificateUpdateModel.Id);


                if (entity.Gem.IsGemLot)
                {
                    var gemLotHeaderStockNumber = entity.Gem.StockNumber.Split("-")[0];
                    var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.StockNumber == gemLotHeaderStockNumber);

                    gemLotHeader.TotalCost = gemLotHeader.TotalCost - entity.Transaction.PaidAmount + certificateUpdateModel.CertificateFee;

                    gemStoContext.Entry(gemLotHeader).State = EntityState.Modified;
                }


                entity.Gem.TotalCost = entity.Gem.TotalCost - entity.Transaction.PaidAmount + certificateUpdateModel.CertificateFee;

                entity.Transaction.PaidAmount = certificateUpdateModel.CertificateFee;
                entity.Transaction.Remark = Remark.Edited;
                entity.Transaction.GemStatus = entity.Gem.GemStatus;
                entity.Transaction.EditedOn = DateTimeOffset.UtcNow;


                entity.ColourId = certificateUpdateModel.ColourId;
                entity.CertificateProviderId = certificateUpdateModel.CertificateproviderId;
                entity.Number = certificateUpdateModel.CerticateNumber;
                entity.IsTreated = certificateUpdateModel.IsTreated;
                entity.OriginId = certificateUpdateModel.OriginId;
                entity.CertifiedDate = certificateUpdateModel.CertifiedDate;
                entity.Description = certificateUpdateModel.Description;
                entity.CertURL = certificateUpdateModel.CertURL;

                gemHistoryList.Add(new GemHistory().CreateCertificateHistory(certificateUpdateModel.CreatedById, GemHistoryStatusEnum.UpdatedCertificateDetails, entity.Id, $"Certificate details updated", entity.Gem.Id, ActionEnum.Edited, certificateUpdateModel.CreatedByName, entity.Gem.GemStatus));


                gemStoContext.Entry(entity).State = EntityState.Modified;
                gemStoContext.GemHistory.AddRange(gemHistoryList);
                await gemStoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ChangeDefaultCertificateAsync(int gemId, int certificateId, string editedeById, string editedByName)
        {
            var gemHistoryList = new List<GemHistory>();

            var updateAllCertifaicteDefaultToFalse = $"UPDATE [dbo].[Certificates] SET IsDefault=0 WHERE GemId={gemId}";
            var updateDefaultCertificate = $"UPDATE [dbo].[Certificates] SET IsDefault=1 WHERE GemId={gemId} AND Id={certificateId}";

            var query = updateAllCertifaicteDefaultToFalse + " " + updateDefaultCertificate;

#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            await gemStoContext.Database.ExecuteSqlCommandAsync(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.

            //await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Certificates] SET IsDefault=0 WHERE GemId={gemId} UPDATE [dbo].[Certificates] SET IsDefault=1 WHERE GemId={gemId} AND Id={certificateId}");

            var gemEntity = await gemStoContext.Gems.AsNoTracking().FirstOrDefaultAsync(f => f.Id == gemId);
            gemHistoryList.Add(new GemHistory().CreateCertificateHistory(editedeById, GemHistoryStatusEnum.UpdatedCertificateDetails, certificateId, $"Default certificate changed", gemId, ActionEnum.Edited, editedByName, gemEntity.GemStatus));
            gemStoContext.GemHistory.AddRange(gemHistoryList);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task AddToCertificationAsync(CertificationCreateModel certificationCreateModel)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistoryList = new List<GemHistory>();

                try
                {
                    bool isAlreadyCertifiedByProvider = false;

                    if (!certificationCreateModel.IsThirdParty)
                    {
                        isAlreadyCertifiedByProvider = await gemStoContext.Certificates
                           .AnyAsync(a => a.GemId == certificationCreateModel.GemId && a.CertificateProviderId == certificationCreateModel.CertificateProviderId && !a.IsDeleted);

                        if (isAlreadyCertifiedByProvider)
                        {
                            var certProvider = await gemStoContext
                                .CertificateProviders.FirstOrDefaultAsync(f => f.Id == certificationCreateModel.CertificateProviderId && !f.IsDeleted);
                            if (!certificationCreateModel.IsAdmin)
                            {
                                throw new ArgumentException($"Already certified by {certProvider.Value + " " + certProvider.Agent}. Contct Administrator for more details");

                            }
                        }

                    }

                    var entity = mapper.Map<Certification>(certificationCreateModel);

                    var gemEntity = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == certificationCreateModel.GemId);
                    var gemStatusBeforeAddingCertification = gemEntity?.GemStatus;
                    if (!certificationCreateModel.IsThirdParty)
                    {
                        gemEntity.GemStatus = gemEntity.GemStatus == GemStatus.Sold ? GemStatus.SoldCP : GemStatus.Certification;
                    }


                    await gemStoContext.Certifications.AddAsync(entity);
                    await gemStoContext.SaveChangesAsync();



                    if (certificationCreateModel.IsAdmin && isAlreadyCertifiedByProvider)
                    {
                        var certProvider = await gemStoContext
                                .CertificateProviders.FirstOrDefaultAsync(f => f.Id == certificationCreateModel.CertificateProviderId && !f.IsDeleted);

                        if (!certificationCreateModel.IsThirdParty)
                        {
                            var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificationCreateModel.CertificateProviderId && !w.IsDeleted);
                            var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                            gemHistoryList.Add(new GemHistory().CreateCertificationHistory(certificationCreateModel.CreatedById, GemHistoryStatusEnum.AddedToCertification, entity.Id, $"Added for certification at {certProvidedBy}", gemEntity.Id, ActionEnum.Created, certificationCreateModel.CreatedByName, (GemStatus)gemStatusBeforeAddingCertification));

                            gemStoContext.GemHistory.AddRange(gemHistoryList);
                        }
                        await gemStoContext.SaveChangesAsync();

                        dbTransaction.Commit();

                        throw new ArgumentException($"Already certified by {certProvider.Value + " " + certProvider.Agent}, but has been added for certification");

                    }

                    if (!certificationCreateModel.IsThirdParty)
                    {
                        var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificationCreateModel.CertificateProviderId && !w.IsDeleted);
                        var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                        gemHistoryList.Add(new GemHistory().CreateCertificationHistory(certificationCreateModel.CreatedById, GemHistoryStatusEnum.AddedToCertification, entity.Id, $"Added for certification at {certProvidedBy}", gemEntity.Id, ActionEnum.Created, certificationCreateModel.CreatedByName, (GemStatus)gemStatusBeforeAddingCertification));

                        gemStoContext.GemHistory.AddRange(gemHistoryList);
                    }


                    await gemStoContext.SaveChangesAsync();

                    dbTransaction.Commit();
                }

                catch (ArgumentException ex)
                {
                    throw;
                }

                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }

        }

        public async Task RemoveGemFromCertificationAsync(int id, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistoryList = new List<GemHistory>();

                try
                {
                    var certificationEntity = gemStoContext.Certifications.Include(i => i.Gem).FirstOrDefaultAsync(f => f.Id == id);

                    var certificationEntityResult = certificationEntity.Result;

                    certificationEntityResult.IsDeleted = true;

                    if (!certificationEntityResult.IsThirdParty)
                    {
                        var gemStatusBeforRemovingFromCertification = certificationEntityResult.Gem.GemStatus;

                        var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificationEntityResult.CertificateProviderId);
                        var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                        gemHistoryList.Add(new GemHistory().CreateCertificationHistory(editedById, GemHistoryStatusEnum.RemovedFromCertification, certificationEntityResult.Id, $"Removed gem from certification at {certProvidedBy}", (int)certificationEntityResult.GemId, ActionEnum.Removed, editedByName, gemStatusBeforRemovingFromCertification));

                        certificationEntityResult.Gem.GemStatus = certificationEntityResult.Gem.GemStatus == GemStatus.Certification ? GemStatus.InStock : (certificationEntityResult.Gem.GemStatus == GemStatus.SoldCP ? GemStatus.Sold : certificationEntityResult.Gem.GemStatus);
                    }
                    certificationEntityResult.EditedById = editedById;
                    certificationEntityResult.EditedOn = DateTimeOffset.UtcNow;

                    gemStoContext.GemHistory.AddRange(gemHistoryList);
                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<PaginationModel<CertificationModel>> GetAllGemToCertificationAsync(PaginationBase paginationBase, CertificationFilterModel certificationFilterModel)
        {

            var query = gemStoContext.Certifications.Where(w => !w.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(paginationBase.SearchQuery))
            {
                query = query.Where(w => EF.Functions.Like(w.Gem.StockNumber, paginationBase.SearchQuery + "%") ||
                                    EF.Functions.Like(w.Owner, paginationBase.SearchQuery + "%")
                                   );
            }


            if (certificationFilterModel.MaxWeight.HasValue)
            {
                query = query.Where(w => (w.Gem.RecutWeight != 0.00m && w.Gem.RecutWeight <= certificationFilterModel.MaxWeight.Value) || (w.Gem.RecutWeight == 0.00m && w.Gem.InitialWeight <= certificationFilterModel.MaxWeight.Value) || w.Weight <= certificationFilterModel.MaxWeight.Value);
            }


            if (certificationFilterModel.MinWeight.HasValue)
            {
                query = query.Where(w => (w.Gem.RecutWeight != 0.00m && w.Gem.RecutWeight >= certificationFilterModel.MinWeight.Value) || (w.Gem.RecutWeight == 0.00m && w.Gem.InitialWeight >= certificationFilterModel.MinWeight.Value) || w.Weight >= certificationFilterModel.MinWeight.Value);
            }

            if (certificationFilterModel.CertificateProviderId.HasValue)
            {
                query = query.Where(w => w.CertificateProviderId == certificationFilterModel.CertificateProviderId);
            }

            if (certificationFilterModel.FromDate.HasValue)
            {
                query = query.Where(w => w.SubmissionDate >= certificationFilterModel.FromDate.Value);
            }

            if (certificationFilterModel.ToDate.HasValue)
            {
                query = query.Where(w => w.SubmissionDate <= certificationFilterModel.ToDate.Value);
            }

            if (!certificationFilterModel.ShowAll)
            {
                query = query.Where(w => !w.IsCertified);
            }

            var totalNumberOfRecord = query.CountAsync();

            if (!certificationFilterModel.IsPrintable)
            {
                query = query.OrderByDescending(o => o.Id).Skip(paginationBase.Skip).Take(paginationBase.Take).AsQueryable();
            }

            var result = query.Select(s => new CertificationModel
            {
                Id = s.Id,
                CertificateProviderName = s.CertificateProvider.Value,
                CertProviderAgent = s.CertificateProvider.Agent ?? "-",
                CertificateProviderId = s.CertificateProviderId,
                StockNumber = s.GemId.HasValue ? s.Gem.StockNumber : s.Owner,
                SubmittedDate = s.SubmissionDate,
                ReceivedDate = s.RecievedDate,
                IsCertified = s.IsCertified,
                GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty,
                GemId = s.GemId.HasValue ? s.Gem.Id : (int?)null,
                Weight = s.GemId.HasValue ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : (decimal)s.Weight,
                ShapePath = s.GemId.HasValue ? (s.Gem.RecutShapeId.HasValue ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,
                IsThirdParty = s.IsThirdParty,
                IsTreated = s.GemId.HasValue ? s.Gem.IsTreated : s.IsTreated,
                Variety = s.GemId.HasValue ? s.Gem.Variety.Value : s.Variety.Value,
                VarietyId = s.VarietyId
            }).AsQueryable();

            var resultSet = await result.AsNoTracking().ToListAsync();

            var resultData = new PaginationModel<CertificationModel>
            {
                Details = resultSet,
                TotalRecords = totalNumberOfRecord.Result
            };
            return resultData;
        }

        public async Task AddCertificatonCertificateAsync(CertificationCertificateCreateModel certificationCertificateCreateModel)
        {
            try
            {
                var certitificationEntity = await gemStoContext.Certifications.Include(i => i.Gem).FirstOrDefaultAsync(f => f.Id == certificationCertificateCreateModel.Id);

                if (certificationCertificateCreateModel.CertifiedDate != null && (certificationCertificateCreateModel.CertifiedDate.Value.Date < certitificationEntity.SubmissionDate.Date))
                {
                    throw new Exception("Submission date cannot exceed received date");
                }

                if (!certificationCertificateCreateModel.IsThirdParty)
                {
                    var mappedcertificateCreateModel = mapper.Map<CertificateCreateModel>(certificationCertificateCreateModel);
                    mappedcertificateCreateModel.GemId = (int)certitificationEntity.GemId;
                    mappedcertificateCreateModel.CertificateProviderId = certitificationEntity.CertificateProviderId;

                    var isSuccess = await CreateAsync(mappedcertificateCreateModel, certificationCertificateCreateModel.Length, certificationCertificateCreateModel.Width, certificationCertificateCreateModel.Depth, certificationCertificateCreateModel.RecutShapeId, certificationCertificateCreateModel.RecutWeight);
                    if (!isSuccess)
                    {
                        throw new Exception("This certificate is already available");
                    }

                    else
                    {
                        certitificationEntity.IsCertified = true;
                        certitificationEntity.Gem.GemStatus = certitificationEntity.Gem.GemStatus == GemStatus.Certification ? GemStatus.InStock : (certitificationEntity.Gem.GemStatus == GemStatus.SoldCP ? GemStatus.Sold : certitificationEntity.Gem.GemStatus);
                        certitificationEntity.RecievedDate = certificationCertificateCreateModel.CertifiedDate;

                        await gemStoContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateCertificationAsync(CertificationUpdateModel certificationUpdateModel)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistoryList = new List<GemHistory>();

                try
                {
                    var certificationEntity = await gemStoContext.Certifications.FirstOrDefaultAsync(f => f.Id == certificationUpdateModel.Id);

                    if (certificationUpdateModel.GemId != null)
                    {
                        var gemEntities = await gemStoContext.Gems.Where(w => w.Id == (int)certificationUpdateModel.GemId || w.Id == certificationEntity.GemId).ToListAsync();

                        //------------------- remove old gem from certification --------------------------------------------------
                        var oldGem = gemEntities.First(f => f.Id == certificationEntity.GemId);

                        var gemStatusBeforRemovingFromCertification = oldGem.GemStatus;

                        var certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificationEntity.CertificateProviderId);
                        var certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                        gemHistoryList.Add(new GemHistory().CreateCertificationHistory(certificationUpdateModel.CreatedById, GemHistoryStatusEnum.RemovedFromCertification, certificationEntity.Id, $"Removed gem from certification at {certProvidedBy}", oldGem.Id, ActionEnum.Removed, certificationUpdateModel.CreatedByName, gemStatusBeforRemovingFromCertification));

                        oldGem.GemStatus = GemStatus.InStock;
                        //------------------- end remove old gem from certification --------------------------------------------------


                        // ------------------ add new gem for certification -------------------------------------------------------------
                        var newGem = gemEntities.First(f => f.Id == (int)certificationUpdateModel.GemId);

                        var gemStatusBeforeAddingCertification = newGem.GemStatus;

                        certificateProvider = await gemStoContext.CertificateProviders.AsNoTracking().FirstOrDefaultAsync(w => w.Id == certificationEntity.CertificateProviderId && !w.IsDeleted);
                        certProvidedBy = certificateProvider.Agent is null ? certificateProvider.Value : certificateProvider.Value + " - " + certificateProvider.Agent;

                        gemHistoryList.Add(new GemHistory().CreateCertificationHistory(certificationUpdateModel.CreatedById, GemHistoryStatusEnum.AddedToCertification, certificationEntity.Id, $"Added for certification at {certProvidedBy}", newGem.Id, ActionEnum.Created, certificationUpdateModel.CreatedByName, gemStatusBeforeAddingCertification));

                        newGem.GemStatus = GemStatus.Certification;
                        // ------------------ end add new gem for certification ----------------------------------------------------------


                        certificationEntity.GemId = (int)certificationUpdateModel.GemId;
                    }

                    certificationEntity.SubmissionDate = certificationUpdateModel.SubmissionDate;
                    certificationEntity.CertificateProviderId = certificationUpdateModel.CertificateProviderId;

                    await gemStoContext.GemHistory.AddRangeAsync(gemHistoryList);

                    await gemStoContext.SaveChangesAsync();
                    dbTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateThirdPartyCertificationAsync(ThirdPartyCertificationUpdateModel thirdPartyCertificationUpdateModel)
        {
            var entity = await gemStoContext.Certifications.FirstOrDefaultAsync(f => f.Id == thirdPartyCertificationUpdateModel.Id);

            entity.CertificateProviderId = thirdPartyCertificationUpdateModel.CertificateProviderId;
            entity.Owner = thirdPartyCertificationUpdateModel.Owner;
            entity.Weight = thirdPartyCertificationUpdateModel.Weight;
            entity.SubmissionDate = thirdPartyCertificationUpdateModel.SubmissionDate;
            entity.ShapeId = thirdPartyCertificationUpdateModel.ShapeId;
            entity.IsTreated = thirdPartyCertificationUpdateModel.IsTreated;
            entity.VarietyId = thirdPartyCertificationUpdateModel.VarietyId;

            gemStoContext.Entry(entity).State = EntityState.Modified;
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<CertificationModel> GetThirdPartyCertificationByIdAsync(int id)
        {
            var entity = await gemStoContext.Certifications.FirstOrDefaultAsync(f => f.Id == id);

            var model = mapper.Map<CertificationModel>(entity);
            return model;
        }

        public async Task AddThirdPartyCertificateAsync(AddThirdPartyCertificateModel addThirdPartyCertificateModel)
        {
            try
            {
                var entity = mapper.Map<ThirdPartyCertificate>(addThirdPartyCertificateModel);

                var isCertificateNumberTaken = await gemStoContext.ThirdPartyCertificates.AnyAsync(a => a.Number == addThirdPartyCertificateModel.Number);

                if (isCertificateNumberTaken)
                {
                    throw new Exception("This certificate number is already taken");
                }
                else
                {
                    var certificationEntity = await gemStoContext.Certifications.FirstOrDefaultAsync(f => f.Id == addThirdPartyCertificateModel.CertificationId);
                    certificationEntity.IsCertified = true;

                    await gemStoContext.ThirdPartyCertificates.AddAsync(entity);
                    await gemStoContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ThirdPartyCertificateModel> GetThirdCertificateByIdAsync(int certificationId)
        {
            var model = await gemStoContext.Certifications.Where(w => w.Id == certificationId).Select(s => new ThirdPartyCertificateModel
            {
                CerticateNumber = s.ThirdPartyCertificate.Number,
                CerticateProvider = s.CertificateProvider.Value,
                CertificateFee = s.ThirdPartyCertificate.Cost,
                CertifiedDate = s.ThirdPartyCertificate.CertifiedDate,
                Color = s.ThirdPartyCertificate.Colour.Value + " [" + s.ThirdPartyCertificate.Colour.Description + "]",
                Depth = s.ThirdPartyCertificate.Depth,
                Description = s.ThirdPartyCertificate.Description,
                IsTreated = s.ThirdPartyCertificate.IsTreated,
                Length = s.ThirdPartyCertificate.Length,
                OriginName = s.ThirdPartyCertificate.Origin.Value,
                ShapeName = s.ThirdPartyCertificate.Shape.Value,
                ShapePath = s.ThirdPartyCertificate.Shape.ImagePath,
                Weight = s.ThirdPartyCertificate.Weight,
                Width = s.ThirdPartyCertificate.Width,
                VarietyName = s.Variety.Value + " [" + s.Variety.Description + "]"
            }).FirstOrDefaultAsync();
            return model;
        }
    }
}

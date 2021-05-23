using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Approval;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class ApprovalService : IApprovalService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;
        private readonly IApproverLookUpService approverLookUpService;

        public ApprovalService(GemStoContext gemStoContext, IMapper mapper, IApproverLookUpService approverLookUpService)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
            this.approverLookUpService = approverLookUpService;
        }

        public async Task AddGemToApprovalAsync(AddGemToApprovalListModel addGemToApprovalListModel)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemEntity = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == addGemToApprovalListModel.GemId);
                    var gemStatusBeforeUpdate = gemEntity.GemStatus;
                    var approval = await gemStoContext.Approvals.AsNoTracking().Include(i => i.Approver).FirstOrDefaultAsync(f => f.Id == addGemToApprovalListModel.ApprovalId);

                    gemEntity.GemStatus = GemStatus.Approval;
                    var entity = mapper.Map<GemApproval>(addGemToApprovalListModel);
                    gemStoContext.Add(entity);
                    await gemStoContext.SaveChangesAsync();
                    var gemHistory = new GemHistory().CreateApprovalHistory(addGemToApprovalListModel.CreatedById, GemHistoryStatusEnum.AddedToApproval, entity.Id, $"Added for approval over {approval.Approver.Name}", gemEntity.Id, ActionEnum.Created, addGemToApprovalListModel.CreatedByName, gemStatusBeforeUpdate);
                    gemStoContext.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task ApprovalSummaryUpdateAsync(ApprovalSummaryUpdateModel approvalSummaryUpdateModel)
        {
            var entity = await gemStoContext.Approvals.FirstOrDefaultAsync(f => f.Id == approvalSummaryUpdateModel.Id);

            entity.Description = approvalSummaryUpdateModel.Description;

            await gemStoContext.SaveChangesAsync();
        }

        public async Task CloseApprovalAsync(Guid approvalId, string editedById, string editedByName)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemHistoryList = new List<GemHistory>();
                    var gemApprovals = await gemStoContext.GemApprovals
                        .Include(i => i.Gem)
                        .Include(i => i.Approval)
                        .ThenInclude(i => i.Approver)
                        .Where(w => w.ApprovalId == approvalId && !w.IsDeleted)
                        .ToListAsync();

                    foreach (var gemApproval in gemApprovals)
                    {
                        gemHistoryList.Add(new GemHistory().CreateApprovalHistory(editedById, GemHistoryStatusEnum.RemovedFromApproval, gemApproval.Id, $"Removed from approval over {gemApproval?.Approval?.Approver?.Name}", gemApproval.Gem.Id, ActionEnum.Removed, editedByName, gemApproval.Gem.GemStatus));
                    }

                    gemStoContext.GemHistory.AddRange(gemHistoryList);
                    var approvalIdAsString = approvalId.ToString();
                    await gemStoContext.Database.ExecuteSqlCommandAsync("[dbo].[spCloseApproval] @p0", parameters: approvalIdAsString);
                    await gemStoContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                transaction.Commit();
            }
        }

        public async Task<string> CreateApprovalSummaryAsync(ApprovalSummaryCreateModel approvalSummaryCreateModel)
        {
            try
            {
                var entity = mapper.Map<Approval>(approvalSummaryCreateModel);

                await approverLookUpService
                    .ChangeApproverProfileStatusAsync(new StatusChangeModel<Guid> { Id = approvalSummaryCreateModel.ApproverId, Status = true });

                await gemStoContext.Approvals.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();

                return entity.Id.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task EditApprivalDetailByIdAsync(ApprovalDetailUpdateModel approvalDetailUpdateModel, string editedById, string editedByName)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemHistoryList = new List<GemHistory>();

                    var entity = await gemStoContext.GemApprovals.Include(i => i.Gem).FirstOrDefaultAsync(f => f.Id == approvalDetailUpdateModel.Id);

                    entity.EditedById = editedById;
                    entity.EditedOn = DateTimeOffset.UtcNow;

                    if (entity.Gem?.SellingPrice != approvalDetailUpdateModel?.SellingPrice)
                    {
                        gemHistoryList.Add(new GemHistory().CreateApprovalHistory(editedById, GemHistoryStatusEnum.EditedGemDetail, entity.Id, $"Edited gem selling price from {entity.Gem?.SellingPrice ?? 0.00m} to { approvalDetailUpdateModel?.SellingPrice ?? 0.00m}", entity.Gem.Id, ActionEnum.Edited, editedByName, entity.Gem.GemStatus));
                    }

                    entity.Gem.SellingPrice = approvalDetailUpdateModel.SellingPrice;
                    entity.Description = (approvalDetailUpdateModel.Description != null && approvalDetailUpdateModel.Description.Length < 1) ? null : approvalDetailUpdateModel.Description;
                    gemStoContext.GemHistory.AddRange(gemHistoryList);
                    await gemStoContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<PaginationModel<ApprovalSummaryModel>> GetAllApprovalSummaryAsync(PaginationBase paginationBase)
        {
            var query = gemStoContext.Approvals.Where(w => !w.IsDeleted && !w.Approver.IsDeleted);

            if (!string.IsNullOrEmpty(paginationBase.SearchQuery))
            {
                var searchQuery = paginationBase.SearchQuery.ToLower();
                query = query.Where(w => EF.Functions.Like(w.Approver.Name, searchQuery + "%")
                );
            }

            var totalNumberOfRecords = await query.AsNoTracking().CountAsync();

            query = query.OrderByDescending(o => o.CreatedOn).Skip(paginationBase.Skip).Take(paginationBase.Take);

            var result = query.Select(s => new ApprovalSummaryModel
            {
                ApproverName = s.Approver.Name,
                Description = s.Description,
                Id = s.Id,
                NumberOfPieces = s.GemApprovals.Count(c => !c.IsDeleted && c.Gem.GemStatus == GemStatus.Approval)
            }).AsQueryable();

            var resultData = await result.AsNoTracking().ToListAsync();

            var approvalSummaryModel = new PaginationModel<ApprovalSummaryModel>
            {
                TotalRecords = totalNumberOfRecords,
                Details = resultData
            };

            return approvalSummaryModel;
        }

        public async Task<PaginationModel<ApprovalGemModel>> GetApprovalDetailByIdAsync(PaginationBase paginationBase, Guid approvalId, bool isPrintable, bool isAll)
        {
            var query = gemStoContext.GemApprovals.Include(i => i.Approval).Where(w => w.ApprovalId == approvalId && w.Gem.IsTreated != null).OrderBy(o => o.CreatedOn).AsQueryable();

            if (!isAll)
            {
                query = query.Where(w => !w.IsDeleted && w.Gem.GemStatus == GemStatus.Approval);
            }

            if (!string.IsNullOrEmpty(paginationBase.SearchQuery))
            {
                query = query.Where(w => EF.Functions.Like(w.Gem.StockNumber, paginationBase.SearchQuery + "%"));
            }

            var totalNumberOfRecords = await query.CountAsync();

            if (isPrintable)
            {
                query = query.Where(w => w.Gem.GemStatus == GemStatus.Approval && !w.IsDeleted);
            }

            else
            {
                query = query.OrderByDescending(o => o.CreatedOn).Skip(paginationBase.Skip).Take(paginationBase.Take);
            }

            var result = query.Select(s => new ApprovalGemModel
            {
                Id = s.Id,
                StockNumber = s.Gem.StockNumber,
                Variety = s.Gem.Variety.Value,
                IsTreated = s.Gem.IsTreated.Value,
                Weight = s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight,
                Shape = s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath,
                Price = s.Gem.TotalCost,
                CreatedOn = s.CreatedOn,
                SellingPrice = s.Gem.SellingPrice,
                Description = s.Description,
                GemId = s.GemId,
                GemIdentity = s.Gem.Identity,
                Seller = s.Gem.SellerId.HasValue ? s.Gem.Seller.Name : s.Gem.SellerName,
                GemStatus = s.Gem.GemStatus,
                IsDeleted = s.IsDeleted,
                Certificate = s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).CertificateProvider.Value + " - " +
                    s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Colour.Value +
                     (s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Origin.Value != null ? " - " + s.Gem.Certificates.FirstOrDefault(f => f.IsDefault && !f.IsDeleted).Origin.Value : string.Empty)
            });

            var resultData = await result.AsNoTracking().ToListAsync();

            var resultSet = new PaginationModel<ApprovalGemModel>()
            {
                TotalRecords = totalNumberOfRecords,
                Details = resultData
            };
            return resultSet;
        }

        public async Task<ApprovalGemModel> GetApprovalGemByIdAsync(int approvalDetailId)
        {
            var entity = await gemStoContext.GemApprovals.Include(i => i.Gem).FirstOrDefaultAsync(f => f.Id == approvalDetailId);
            var model = new ApprovalGemModel
            {
                Id = entity.Id,
                SellingPrice = entity.Gem.SellingPrice,
                Description = entity.Description
            };

            return model;
        }

        public async Task<ApprovalSummaryModel> GetApprovalSummaryByIdIdAsync(Guid approvalId)
        {
            var entity = await gemStoContext.Approvals.Include(i => i.Approver).FirstOrDefaultAsync(w => w.Id == approvalId);

            var model = mapper.Map<ApprovalSummaryModel>(entity);
            return model;
        }

        public async Task RemoveGemFromApprovalAsync(int id, string editedById, string editedByName)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemHistoryList = new List<GemHistory>();

                    var entity = await gemStoContext.GemApprovals
                        .Include(i => i.Gem)
                        .Include(i => i.Approval)
                            .ThenInclude(i => i.Approver)
                        .FirstOrDefaultAsync(f => f.Id == id);

                    gemHistoryList.Add(new GemHistory().CreateApprovalHistory(editedById, GemHistoryStatusEnum.RemovedFromApproval, entity.Id, $"Removed from approval over {entity?.Approval?.Approver?.Name}", entity.Gem.Id, ActionEnum.Removed, editedByName, entity.Gem.GemStatus));


                    entity.IsDeleted = true;
                    entity.Gem.GemStatus = GemStatus.InStock;
                    entity.EditedOn = DateTimeOffset.UtcNow;
                    entity.EditedById = editedById;
                    gemStoContext.GemHistory.AddRange(gemHistoryList);
                    await gemStoContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

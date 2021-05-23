using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Sale;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class SalesService : ISalesService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public SalesService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task CreateSaleAsync(SaleCreateModel saleCreateModel)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();
                var listOFGemStatusBeforeSale = new List<KeyValuePair<int, GemStatus>>();

                try
                {
                    // --------------  start payment check -----------------------------
                    var paymentStatus = new PaymentStatus();

                    var totalAmount = saleCreateModel.GemSales.Sum(s => s.Amount);

                    if (!saleCreateModel.IsSingleSale)
                    {
                        if (saleCreateModel.GrossTotal.HasValue && saleCreateModel.AmmountPaid > saleCreateModel.GrossTotal)
                        {
                            throw new Exception("Initial payment exceeds the total sales amount");
                        }

                        else if (!saleCreateModel.GrossTotal.HasValue && saleCreateModel.AmmountPaid > totalAmount)
                        {
                            throw new Exception("Initial payment exceeds the total sales amount");
                        }
                    }
                    else
                    {
                        if (saleCreateModel.AmmountPaid > totalAmount)
                        {
                            throw new Exception("Initial payment exceeds the total sales amount");
                        }
                    }

                    if (saleCreateModel.AmmountPaid == saleCreateModel.GrossTotal ||
                        (saleCreateModel.AmmountPaid == totalAmount) && saleCreateModel.GemSales.All(a => a.Amount != null))
                    {
                        paymentStatus = PaymentStatus.Paid;
                    }

                    else if (saleCreateModel.AmmountPaid > 0)
                    {
                        paymentStatus = PaymentStatus.Partial;
                    }

                    else
                    {
                        paymentStatus = PaymentStatus.Unpaid;
                    }
                    //-------------- end payment check ----------------------------------------

                    var saleHeader = new Sale
                    {
                        Number = saleCreateModel.SaleNumber,
                        CreatedById = saleCreateModel.CreatedById,
                        CreatedOn = saleCreateModel.Date,
                        Commission = saleCreateModel.Commission,
                        Note = (saleCreateModel.Note != null && saleCreateModel.Note.Length != 0) ? saleCreateModel.Note : null
                    };


                    //------------------------------------- single gem sale start --------------------------------
                    if (saleCreateModel.IsSingleSale && !saleCreateModel.IsThirdParty)
                    {
                        foreach (var item in saleCreateModel.GemSales)
                        {


                            var export = await gemStoContext.GemExports
                                .AsNoTracking()
                                .FirstOrDefaultAsync(f => !f.IsDeleted && f.GemId == item.GemId && !f.Export.IsExportClosed);

                            GemApproval gemApproval = null;
                            if (export is null)
                            {
                                gemApproval = await gemStoContext.GemApprovals
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(f => !f.IsDeleted && f.GemId == item.GemId && !f.Approval.IsDeleted && !f.Approval.Approver.IsDeleted);
                            }

                            var gemSales = new GemSales
                            {
                                CreatedById = saleCreateModel.CreatedById,
                                TotalAmount = item.Amount,
                                SaleId = saleHeader.Id,
                                GemId = item.GemId,
                                IsCertificatePending = item.IsCertificatePending,
                                SaleNumber = saleCreateModel.SaleNumber.ToString(),
                                NumberOfPieces = 1,
                                IsThirdParty = false,
                                IsSingleSale = true,
                                BuyerId = saleCreateModel.BuyerId,
                                BuyerName = saleCreateModel.BuyerName,
                                PaymentStatus = paymentStatus,
                                SellingRate = saleCreateModel.SellingRate,
                                TotalAmountPaid = saleCreateModel.PaymentType == PaymentType.cash ? saleCreateModel.AmmountPaid : 0.00m,
                                GemExportId = export?.Id,
                                GemApprovalId = gemApproval?.Id
                            };

                            var gem = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == item.GemId);
                            listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>(gem.Id, gem.GemStatus));
                            gem.GemStatus = GemStatus.Sold;

                            saleHeader.GemSales.Add(gemSales);

                        }
                    }
                    //-------------------------------------- single gem sale end ---------------------------------

                    //------------------------------------- lot gem sale start --------------------------------
                    else if (!saleCreateModel.IsSingleSale)
                    {
                        var gemSalesHeader = new GemSales
                        {
                            CreatedById = saleCreateModel.CreatedById,
                            TotalAmount = totalAmount == 0.00m ? saleCreateModel.GrossTotal : totalAmount,
                            SaleId = saleHeader.Id,
                            GemId = null,
                            IsCertificatePending = null,
                            SaleNumber = saleCreateModel.SaleNumber.ToString(),
                            NumberOfPieces = saleCreateModel.GemSales.Count,
                            IsThirdParty = false,
                            IsSingleSale = false,
                            BuyerId = saleCreateModel.BuyerId,
                            BuyerName = saleCreateModel.BuyerName,
                            PaymentStatus = paymentStatus,
                            SellingRate = saleCreateModel.SellingRate,
                            TotalAmountPaid = saleCreateModel.PaymentType == PaymentType.cash ? saleCreateModel.AmmountPaid : 0.00m,
                        };

                        saleHeader.GemSales.Add(gemSalesHeader);

                        var gemIds = saleCreateModel.GemSales.Select(s => s.GemId).ToList();

                        var exports = await gemStoContext.GemExports
                               .Where(f => !f.IsDeleted && gemIds.Contains((int)f.GemId) && !f.Export.IsExportClosed).ToListAsync();

                        var gemApprovals = await gemStoContext.GemApprovals
                                                   .Where(f => !f.IsDeleted && gemIds.Contains((int)f.GemId) && !f.Approval.IsDeleted && !f.Approval.Approver.IsDeleted).ToListAsync();


                        int counter = 1;
                        foreach (var item in saleCreateModel.GemSales)
                        {
                            var gemSales = new GemSales
                            {
                                CreatedById = saleCreateModel.CreatedById,
                                TotalAmount = item?.Amount,
                                SaleId = saleHeader.Id,
                                GemId = item.GemId,
                                IsCertificatePending = item.IsCertificatePending,
                                SaleNumber = saleCreateModel.SaleNumber.ToString() + "-" + counter++.ToString(),
                                NumberOfPieces = 1,
                                IsThirdParty = false,
                                IsSingleSale = false,
                                BuyerId = saleCreateModel.BuyerId,
                                BuyerName = saleCreateModel.BuyerName,
                                PaymentStatus = paymentStatus,
                                SellingRate = saleCreateModel.SellingRate,
                                TotalAmountPaid = -1,
                                GemExportId = exports.Any() ? exports.FirstOrDefault(f => f.GemId == item.GemId)?.Id : (int?)null,
                                GemApprovalId = gemApprovals.Any() ? gemApprovals.FirstOrDefault(f => f.GemId == item.GemId)?.Id : (int?)null,
                            };

                            var gem = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.Id == item.GemId);
                            listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>(gem.Id, gem.GemStatus));
                            gem.GemStatus = GemStatus.Sold;
                            saleHeader.GemSales.Add(gemSales);
                        }
                        saleHeader.GemSales.Reverse();
                    }
                    //-------------------------------------- lot gem sale end ---------------------------------

                    //----------------------------------- third party sale start------------------------------------
                    else if (saleCreateModel.IsSingleSale && saleCreateModel.IsThirdParty)
                    {

                        if (saleCreateModel.GemExportId.HasValue)
                        {
                            var gemExportEntity = await gemStoContext.GemExports.FirstOrDefaultAsync(f => f.Id == saleCreateModel.GemExportId.Value);
                            gemExportEntity.GemStatus = GemStatus.Sold;
                            saleCreateModel.ThirdPartyOwner = gemExportEntity.Owner;
                            saleCreateModel.ShapeId = gemExportEntity.ShapeId;
                            saleCreateModel.IsTreated = gemExportEntity.IsTreated;
                            saleCreateModel.VarietyId = gemExportEntity.VarietyId;
                            saleCreateModel.Weight = gemExportEntity.Weight;
                        }

                        foreach (var item in saleCreateModel.GemSales)
                        {
                            var gemSales = new GemSales
                            {
                                CreatedById = saleCreateModel.CreatedById,
                                TotalAmount = item.Amount,
                                SaleId = saleHeader.Id,
                                IsCertificatePending = item.IsCertificatePending,
                                SaleNumber = saleCreateModel.SaleNumber.ToString(),
                                IsThirdParty = true,
                                ThirdPartyOwner = saleCreateModel.ThirdPartyOwner,
                                BuyerId = saleCreateModel.BuyerId,
                                BuyerName = saleCreateModel.BuyerName,
                                CreatedOn = saleCreateModel.Date,
                                IsTreated = saleCreateModel.IsTreated,
                                GemId = null,
                                IsSingleSale = true,
                                NumberOfPieces = 1,
                                PaymentStatus = paymentStatus,
                                SellingRate = saleCreateModel.SellingRate,
                                ShapeId = saleCreateModel.ShapeId,
                                TotalAmountPaid = saleCreateModel.PaymentType == PaymentType.cash ? saleCreateModel.AmmountPaid : 0.00m,
                                VarietyId = saleCreateModel.VarietyId,
                                Weight = saleCreateModel.Weight,
                                GemExportId = saleCreateModel.GemExportId
                            };
                            saleHeader.GemSales.Add(gemSales);
                        }
                    }
                    //------------------------------------- thir party sale end ---------------------------------

                    var saleHeaderEntity = gemStoContext.Add(saleHeader);

                    if (saleCreateModel.AmmountPaid != 0.00m)
                    {
                        saleHeader.SalePayments = new List<SalePayment>
                        {
                            new SalePayment
                            {
                            GemSalesId = !saleCreateModel.IsSingleSale ? saleHeaderEntity.Entity.GemSales.LastOrDefault().Id : saleHeaderEntity.Entity.GemSales.FirstOrDefault().Id,
                            CreatedById = saleCreateModel.CreatedById,
                            AmmountPaid = saleCreateModel.AmmountPaid,
                            PaymentType = saleCreateModel.PaymentType,
                            IsPaymentApproved = saleCreateModel.PaymentType == PaymentType.cash ? true : false,
                            CreatedOn = saleCreateModel.Date
                            }
                        };
                    }

                    await gemStoContext.SaveChangesAsync();

                    var listOfGemSale = saleHeader.GemSales.Where(w => w.GemId != null).ToList();

                    foreach (var sale in listOfGemSale)
                    {
                        var gemStatusBeforeSale = listOFGemStatusBeforeSale.FirstOrDefault(f => f.Key == sale.GemId);
                        var soldFrom = gemStatusBeforeSale.Value == GemStatus.Exported ? "export" : (gemStatusBeforeSale.Value == GemStatus.Approval ? "approval" : "stock");

                        gemHistory.Add(new GemHistory().CreateSaleHistory(saleCreateModel.CreatedById, GemHistoryStatusEnum.AddedToSale, sale.Id, $"Added to sale from {soldFrom} on Sale Id:- {saleCreateModel.SaleNumber}", (int)sale.GemId, ActionEnum.Created, saleCreateModel.CreatedByName, gemStatusBeforeSale.Value));
                    }

                    gemStoContext.GemHistory.AddRange(gemHistory);
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

        public async Task<PaginationModel<SaleModel>> GetAllSalesAsync(PaginationBase paginationBase)
        {
            var query = gemStoContext.GemSales.Include(i => i.Sale).Include(i => i.SalePayments).AsQueryable();

            if (!string.IsNullOrEmpty(paginationBase.SearchQuery))
            {
                var q = paginationBase.SearchQuery.ToLower();
                query = query.Where(w =>
                                      EF.Functions.Like(w.SaleNumber, q + "%") ||
                                      EF.Functions.Like(w.Gem.StockNumber, q + "%") ||
                                      EF.Functions.Like(w.ThirdPartyOwner, q + "%") ||
                                      EF.Functions.Like(w.Buyer.Name, q + "%") ||
                                      EF.Functions.Like(w.BuyerName, q + "%") ||
                                      w.PaymentStatus == GetEnumValueByDescription.GetValueFromDescription<PaymentStatus>(q)
                                   );
            }

            var totalNumberOfRecord = await query.CountAsync();

            query = query.OrderByDescending(o => o.Id).Skip(paginationBase.Skip).Take(paginationBase.Take);

            var result = query.Select(s => new SaleModel
            {
                Id = s.Id,
                SaleId = s.SaleNumber,
                Variety = s.GemId != null ? s.Gem.Variety.Value : (s.GemId == null && !s.IsSingleSale ? null : s.Variety.Value),
                Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : (s.GemId == null && !s.IsSingleSale ? null : s.Shape.ImagePath),
                StockNo = s.GemId != null ? s.Gem.StockNumber : (s.GemId == null && !s.IsSingleSale ? null : s.ThirdPartyOwner),
                IsTreated = s.GemId != null ? s.Gem.IsTreated : (s.GemId == null && !s.IsSingleSale ? null : s.IsTreated),
                NoOfPieces = s.NumberOfPieces,
                Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                Weight = s.GemId != null ? (s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight) : (s.GemId == null && !s.IsSingleSale ? null : s.Weight),
                Date = s.Sale.CreatedOn,
                SellingRate = s.SellingRate,
                Price = s.TotalAmount,
                TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                PaymentStatus = s.PaymentStatus,
                IsSingleSale = s.IsSingleSale,
                IsCertificatePending = s.IsCertificatePending,
                IsThirdParty = s.IsThirdParty,
                Share = s.GemId != null ? s.Gem.Share : 0.00m,
                IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                BuyerId = s.BuyerId,
                Commission = s.Sale.Commission,
                GemId = s.GemId.HasValue ? s.GemId : null,
                GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty,
                IsDeleted = s.IsDeleted,
                Note = s.Sale.Note
            });

            var resultData = await result.AsNoTracking().ToListAsync();

            var resultSet = new PaginationModel<SaleModel>
            {
                Details = resultData,
                TotalRecords = totalNumberOfRecord
            };

            return resultSet;
        }

        public async Task<int> GetLastSaleNumberAsync()
        {
            var lastSale = await gemStoContext.Sales.OrderBy(o => o.Number).LastOrDefaultAsync();
            return lastSale == null ? 2000 : ++lastSale.Number;
        }

        public async Task<SaleModel> GetSaleByIdAsync(int saleId)
        {
            var query = gemStoContext.GemSales.Include(i => i.Sale).Include(i => i.SalePayments).Where(w => w.Id == saleId);


            var result = await query.Select(s => new SaleModel
            {
                Id = s.Id,
                SaleId = s.SaleNumber,
                Variety = s.GemId != null ? s.Gem.Variety.Value : (s.GemId == null && !s.IsSingleSale ? null : s.Variety.Value),
                Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : (s.GemId == null && !s.IsSingleSale ? null : s.Shape.ImagePath),
                StockNo = s.GemId != null ? s.Gem.StockNumber : (s.GemId == null && !s.IsSingleSale ? null : s.ThirdPartyOwner),
                IsTreated = s.GemId != null ? s.Gem.IsTreated : (s.GemId == null && !s.IsSingleSale ? null : s.IsTreated),
                NoOfPieces = s.NumberOfPieces,
                Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                Weight = s.GemId != null ? (s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight) : (s.GemId == null && !s.IsSingleSale ? null : s.Weight),
                Date = s.Sale.CreatedOn,
                SellingRate = s.SellingRate,
                Price = s.TotalAmount,
                TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                PaymentStatus = s.PaymentStatus,
                IsSingleSale = s.IsSingleSale,
                IsCertificatePending = s.IsCertificatePending,
                IsThirdParty = s.IsThirdParty,
                Share = s.GemId != null ? s.Gem.Share : 0.00m,
                IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                BuyerId = s.BuyerId,
                Commission = s.Sale.Commission,
                VarietyId = s.GemId != null ? s.Gem.VarietyId : s.VarietyId,
                ShapeId = s.GemId != null ? s.Gem.ShapeId : s.ShapeId,
                IsDeleted = s.IsDeleted,
                Note = s.Sale.Note,
                TotalAmountPaid = s.TotalAmountPaid
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task RemoveGemFromSaleAsync(int gemSaleId, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();
                var listOFGemStatusBeforeSale = new List<KeyValuePair<int, GemStatus>>();

                try
                {
                    var entity = await gemStoContext.GemSales.Include(i => i.GemApproval).Include(i => i.GemExport).ThenInclude(i => i.Export).Include(i => i.Sale).Include(i => i.Gem)
                                       .FirstOrDefaultAsync(f => f.Id == gemSaleId);

                    if (entity.IsSingleSale)
                    {
                        entity.Sale.IsDeleted = entity.IsDeleted = true;

                        if (entity.GemExportId != null && !entity.GemExport.Export.IsExportClosed)
                        {
                            if (entity.GemExport.IsThirdParty)
                            {
                                entity.GemExport.GemStatus = GemStatus.Exported;
                            }
                            else
                            {
                                listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>((int)entity.GemId, entity.Gem.GemStatus));

                                entity.Gem.GemStatus = GemStatus.Exported;
                            }
                        }

                        if (entity.GemApprovalId != null)
                        {
                            listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>((int)entity.GemId, entity.Gem.GemStatus));

                            entity.Gem.GemStatus = GemStatus.InStock;
                            entity.GemApproval.IsDeleted = true;
                        }

                        else if (!entity.IsThirdParty)
                        {
                            listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>((int)entity.GemId, entity.Gem.GemStatus));

                            entity.Gem.GemStatus = GemStatus.InStock;
                        }

                        if (!entity.IsThirdParty)
                        {

                            var gemStatusBeforeReturningFromSale = listOFGemStatusBeforeSale.FirstOrDefault(f => f.Key == entity.GemId);

                            gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.RemovedFromSale, entity.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number}", (int)entity.GemId, ActionEnum.Removed, editedByName, gemStatusBeforeReturningFromSale.Value));
                        }
                    }
                    else if (!entity.IsSingleSale)
                    {
                        var allSalGemsInLot = await gemStoContext.GemSales.Include(i => i.GemApproval).Include(i => i.GemExport).ThenInclude(i => i.Export).Include(i => i.Gem).Include(i => i.Sale).Where(f => f.SaleId == entity.SaleId).ToListAsync();

                        if (entity.GemId == null)
                        {
                            entity.Sale.IsDeleted = true;
                            foreach (var saleItem in allSalGemsInLot)
                            {
                                var gemStatus = GemStatus.InStock;
                                saleItem.IsDeleted = true;

                                if (saleItem.GemExportId != null && !saleItem.GemExport.Export.IsExportClosed)
                                {
                                    gemStatus = saleItem.Gem.GemStatus;
                                    saleItem.Gem.GemStatus = GemStatus.Exported;
                                }
                                else if (saleItem.GemApprovalId != null)
                                {
                                    gemStatus = saleItem.Gem.GemStatus;
                                    saleItem.Gem.GemStatus = GemStatus.InStock;
                                    saleItem.GemApproval.IsDeleted = true;
                                }
                                else if (saleItem.GemId != null)
                                {
                                    gemStatus = saleItem.Gem.GemStatus;
                                    saleItem.Gem.GemStatus = GemStatus.InStock;
                                }

                                if (saleItem.GemId != null)
                                {
                                    gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.RemovedFromSale, saleItem.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number}", (int)saleItem.GemId, ActionEnum.Removed, editedByName, gemStatus));
                                }
                            }
                        }

                        else
                        {
                            var saleGem = allSalGemsInLot.FirstOrDefault(f => f.Id == gemSaleId);

                            var gemStatusBeforeRemovingFromSale = saleGem.Gem.GemStatus;

                            saleGem.Gem.GemStatus = (saleGem.GemExportId != null && !saleGem.GemExport.Export.IsExportClosed) ? GemStatus.Exported : GemStatus.InStock;
                            saleGem.IsDeleted = true;

                            if (saleGem.GemApprovalId != null)
                            {
                                saleGem.GemApproval.IsDeleted = true;
                            }

                            gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.RemovedFromSale, saleGem.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number}", (int)saleGem.GemId, ActionEnum.Removed, editedByName, gemStatusBeforeRemovingFromSale));

                            var lotHeader = allSalGemsInLot.FirstOrDefault(f => f.GemId == null);
                            lotHeader.TotalAmount -= saleGem.TotalAmount ?? 0.00m;
                            if (lotHeader.NumberOfPieces == 0)
                            {
                                lotHeader.Sale.IsDeleted = lotHeader.IsDeleted = true;
                            }
                        }
                    }

                    entity.EditedById = editedById;
                    entity.EditedOn = DateTimeOffset.UtcNow;
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

        public async Task UpdateSaleAsync(SaleUpdateModel saleUpdateModel)
        {
            saleUpdateModel.Note = (saleUpdateModel.Note != null && saleUpdateModel.Note.Length != 0) ? saleUpdateModel.Note : null;
            var gemSaleEntity = await gemStoContext.GemSales.Include(i => i.Sale).Include(i => i.SalePayments)
                                .FirstOrDefaultAsync(f => f.Id == saleUpdateModel.Id && !f.IsDeleted);

            if (!gemSaleEntity.IsSingleSale && gemSaleEntity.GemId != null)
            {
                gemSaleEntity.IsCertificatePending = saleUpdateModel.IsCertificatePending;
                gemSaleEntity.Sale.Note = saleUpdateModel.Note;

                if (gemSaleEntity.TotalAmount != saleUpdateModel.Price)
                {

                    var saleHeader = await gemStoContext.GemSales
                   .FirstOrDefaultAsync(f => !f.IsSingleSale && f.GemId == null && f.SaleId == gemSaleEntity.SaleId);

                    saleHeader.TotalAmount += saleUpdateModel.Price - gemSaleEntity.TotalAmount;
                    gemSaleEntity.TotalAmount = saleUpdateModel.Price;

                    var allSalePayements = await gemStoContext.SalePayments.Where(w => !w.IsDeleted && w.SaleId == saleHeader.SaleId).ToListAsync();
                    var allSalePayementsAmountPaid = allSalePayements.Sum(s => s.AmmountPaid);
                    if (allSalePayementsAmountPaid > saleHeader.TotalAmount)
                    {
                        throw new Exception("Total amount paid exceeds the new price. Remove few payments to continue");
                    }
                    else
                    {
                        gemSaleEntity.PaymentStatus = allSalePayements.Count == 0 ? PaymentStatus.Unpaid :
                                                  (saleHeader.TotalAmount == allSalePayementsAmountPaid ? PaymentStatus.Paid : PaymentStatus.Partial);

                        var gemLotSale = await gemStoContext.GemSales.Where(w => !w.IsDeleted && w.SaleId == saleHeader.SaleId).ToListAsync();

                        gemLotSale.ForEach(f => f.PaymentStatus = gemSaleEntity.PaymentStatus);
                    }
                }
            }

            else
            {

                gemSaleEntity.Sale.CreatedOn = saleUpdateModel.SaleDate;
                gemSaleEntity.IsCertificatePending = saleUpdateModel.IsCertificatePending;
                gemSaleEntity.Sale.Commission = saleUpdateModel.Commission;
                gemSaleEntity.Sale.Note = saleUpdateModel.Note;

                if (gemSaleEntity.TotalAmount != saleUpdateModel.Price || gemSaleEntity.SellingRate != saleUpdateModel.SellingRate || gemSaleEntity.BuyerId != saleUpdateModel.BuyerId || gemSaleEntity.BuyerName != saleUpdateModel.BuyerName)
                {
                    gemSaleEntity.SellingRate = saleUpdateModel.SellingRate;
                    gemSaleEntity.BuyerId = saleUpdateModel.BuyerId;
                    gemSaleEntity.BuyerName = saleUpdateModel.BuyerName;

                    var allSalePayements = gemSaleEntity.SalePayments.Where(w => !w.IsDeleted).ToList();
                    var allSalePayementsAmountPaid = allSalePayements.Sum(s => s.AmmountPaid);
                    if (saleUpdateModel.Price < gemSaleEntity.TotalAmount)
                    {
                        if (allSalePayementsAmountPaid > saleUpdateModel.Price)
                        {
                            throw new Exception("Total amount paid exceeds the new price. Remove few payments to continue");
                        }
                    }

                    gemSaleEntity.TotalAmount = saleUpdateModel.Price;
                    gemSaleEntity.PaymentStatus = allSalePayements.Count == 0 ? PaymentStatus.Unpaid :
                                                  (gemSaleEntity.TotalAmount == allSalePayementsAmountPaid ? PaymentStatus.Paid : PaymentStatus.Partial);


                    if (!gemSaleEntity.IsSingleSale)
                    {
                        var allGemsInSale = await gemStoContext.GemSales.Where(w => w.SaleId == gemSaleEntity.SaleId && w.GemId != null).ToListAsync();

                        foreach (var sale in allGemsInSale)
                        {
                            sale.PaymentStatus = gemSaleEntity.PaymentStatus;
                            sale.SellingRate = saleUpdateModel.SellingRate;
                            sale.BuyerId = saleUpdateModel.BuyerId;
                            sale.BuyerName = saleUpdateModel.BuyerName;
                        }
                    }
                }
            }

            await gemStoContext.SaveChangesAsync();
        }

        public async Task UpdateThirdPartySaleAsync(ThirdPartySaleUpdateModel thirdPartySaleUpdateModel, string editedBy)
        {
            var thirdPartySaleEntity = await gemStoContext.GemSales.Include(i => i.Sale).FirstOrDefaultAsync(f => f.Id == thirdPartySaleUpdateModel.Id);

            thirdPartySaleEntity.BuyerName = thirdPartySaleUpdateModel.BuyerName;
            thirdPartySaleEntity.BuyerId = thirdPartySaleUpdateModel.BuyerId;
            thirdPartySaleEntity.Sale.CreatedOn = thirdPartySaleUpdateModel.SaleDate;
            thirdPartySaleEntity.SellingRate = thirdPartySaleUpdateModel.SellingRate;
            thirdPartySaleEntity.TotalAmount = thirdPartySaleUpdateModel.Price;
            thirdPartySaleEntity.ThirdPartyOwner = thirdPartySaleUpdateModel.Owner;
            thirdPartySaleEntity.Weight = thirdPartySaleUpdateModel.Weight;
            thirdPartySaleEntity.VarietyId = thirdPartySaleUpdateModel.VarietyId;
            thirdPartySaleEntity.IsTreated = thirdPartySaleUpdateModel.IsTreated;
            thirdPartySaleEntity.IsCertificatePending = thirdPartySaleUpdateModel.IsCertificatePending;
            thirdPartySaleEntity.Sale.Commission = thirdPartySaleUpdateModel.Commission;
            thirdPartySaleEntity.ShapeId = thirdPartySaleUpdateModel.ShapeId;
            thirdPartySaleEntity.Sale.Note = (thirdPartySaleUpdateModel.Note != null && thirdPartySaleUpdateModel.Note.Length != 0) ? thirdPartySaleUpdateModel.Note : null;

            thirdPartySaleEntity.EditedById = editedBy;
            thirdPartySaleEntity.EditedOn = DateTimeOffset.UtcNow;

            await gemStoContext.SaveChangesAsync();
        }

        public async Task<PaginationModel<SaleModel>> GlobalSearchAsync(SaleGlobalSearchFilterModel globalSearchFilterModel, int skip, int take)
        {
            try
            {
                var query = gemStoContext.GemSales.Where(w => !w.IsDeleted).AsQueryable();

                if (globalSearchFilterModel.CertificateProviderId != null && globalSearchFilterModel.CertificateProviderId.Count() != 0)
                {
                    query = query.Where(w => w.Gem.Certificates.Any(a => globalSearchFilterModel.CertificateProviderId.Contains(a.CertificateProviderId)));
                }


                if (globalSearchFilterModel.MaxCost.HasValue)
                {
                    query = query.Where(w => w.TotalAmount <= globalSearchFilterModel.MaxCost.Value);
                }

                if (globalSearchFilterModel.MaxWeight.HasValue)
                {
                    query = query.Where(w => ((w.Gem.RecutWeight != 0.00m && w.Gem.RecutWeight <= globalSearchFilterModel.MaxWeight.Value) || (w.Gem.RecutWeight == 0.00m && w.Gem.InitialWeight <= globalSearchFilterModel.MaxWeight.Value)) || w.Weight <= globalSearchFilterModel.MaxWeight.Value);
                }

                if (globalSearchFilterModel.MinCost.HasValue)
                {
                    query = query.Where(w => w.TotalAmount >= globalSearchFilterModel.MinCost.Value);
                }

                if (globalSearchFilterModel.MinWeight.HasValue)
                {
                    query = query.Where(w => ((w.Gem.RecutWeight != 0.00m && w.Gem.RecutWeight >= globalSearchFilterModel.MinWeight.Value) || (w.Gem.RecutWeight == 0.00m && w.Gem.InitialWeight >= globalSearchFilterModel.MinWeight.Value)) || w.Weight >= globalSearchFilterModel.MinWeight.Value);
                }

                if (globalSearchFilterModel.PaymentStatus != null && globalSearchFilterModel.PaymentStatus.Count() != 0)
                {
                    query = query.Where(w => globalSearchFilterModel.PaymentStatus.Contains(w.PaymentStatus));
                }


                if (!string.IsNullOrEmpty(globalSearchFilterModel.Buyer))
                {
                    query = query.Where(w => w.BuyerName == globalSearchFilterModel.Buyer || w.Buyer.Name == globalSearchFilterModel.Buyer);
                }

                if (globalSearchFilterModel.ShapeId != null && globalSearchFilterModel.ShapeId.Count() != 0)
                {
                    query = query.Where(w => ((globalSearchFilterModel.ShapeId.Contains(w.Gem.RecutShapeId) && w.Gem.RecutShapeId != null) || globalSearchFilterModel.ShapeId.Contains(w.Gem.ShapeId) && w.Gem.RecutShapeId == null) || globalSearchFilterModel.ShapeId.Contains(w.ShapeId));
                }

                if (globalSearchFilterModel.VarietyId != null && globalSearchFilterModel.VarietyId.Count() != 0)
                {
                    query = query.Where(w => globalSearchFilterModel.VarietyId.Contains(w.VarietyId) || globalSearchFilterModel.VarietyId.Contains(w.Gem.VarietyId));
                }

                if (!string.IsNullOrEmpty(globalSearchFilterModel.CertificateNumber))
                {
                    query = query.Where(w => w.Gem.Certificates.Any(a => a.Number == globalSearchFilterModel.CertificateNumber));
                }


                if (globalSearchFilterModel.IsNatural.HasValue && !globalSearchFilterModel.IsHeated.HasValue)
                {
                    query = query.Where(w => w.IsTreated == false || w.Gem.IsTreated == false);
                }

                if (globalSearchFilterModel.IsHeated.HasValue && !globalSearchFilterModel.IsNatural.HasValue)
                {
                    query = query.Where(w => w.IsTreated == true || w.Gem.IsTreated == true);
                }

                if (globalSearchFilterModel.SaleFrom.HasValue)
                {
                    query = query.Where(w => w.Sale.CreatedOn >= globalSearchFilterModel.SaleFrom.Value);
                }

                if (globalSearchFilterModel.SaleTo.HasValue)
                {
                    query = query.Where(w => w.Sale.CreatedOn <= globalSearchFilterModel.SaleTo.Value);
                }

                if (!string.IsNullOrEmpty(globalSearchFilterModel.CertificateNumber))
                {
                    query = query.Where(w => w.Gem.Certificates.Any(a => a.Number == globalSearchFilterModel.CertificateNumber));
                }

                if (globalSearchFilterModel.IsThirdParty)
                {
                    query = query.Where(w => w.IsThirdParty);
                }

                if (globalSearchFilterModel.IsCertificatePending)
                {
                    query = query.Where(w => w.IsCertificatePending.Value);
                }


                var totalNumberOfRecord = await query.AsNoTracking().CountAsync();

                query = query.OrderByDescending(x => x.Id).Skip(skip).Take(take);

                var result = query.Select(s => new SaleModel
                {
                    Id = s.Id,
                    SaleId = s.SaleNumber,
                    Variety = s.GemId != null ? s.Gem.Variety.Value : (s.GemId == null && !s.IsSingleSale ? null : s.Variety.Value),
                    Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : (s.GemId == null && !s.IsSingleSale ? null : s.Shape.ImagePath),
                    StockNo = s.GemId != null ? s.Gem.StockNumber : (s.GemId == null && !s.IsSingleSale ? null : s.ThirdPartyOwner),
                    IsTreated = s.GemId != null ? s.Gem.IsTreated : (s.GemId == null && !s.IsSingleSale ? null : s.IsTreated),
                    NoOfPieces = s.NumberOfPieces,
                    Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                    Weight = s.GemId != null ? (s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight) : (s.GemId == null && !s.IsSingleSale ? null : s.Weight),
                    Date = s.Sale.CreatedOn,
                    SellingRate = s.SellingRate,
                    Price = s.TotalAmount,
                    TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                    PaymentStatus = s.PaymentStatus,
                    IsSingleSale = s.IsSingleSale,
                    IsCertificatePending = s.IsCertificatePending,
                    IsThirdParty = s.IsThirdParty,
                    Share = s.GemId != null ? s.Gem.Share : 0.00m,
                    IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                    BuyerId = s.BuyerId,
                    Commission = s.Sale.Commission,
                    GemId = s.GemId.HasValue ? s.GemId : null,
                    GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty,
                });

                var resultData = await result.AsNoTracking().ToListAsync();

                var resultSet = new PaginationModel<SaleModel>
                {
                    Details = resultData,
                    TotalRecords = totalNumberOfRecord
                };

                return resultSet;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<KeyValuePair<int, string>>> GetAllSaleNumbersAsync(string saleNumber)
        {
            var allSaleNumbers = await gemStoContext.GemSales.Where(w => !w.IsThirdParty && !w.IsDeleted && (w.GemId != null || w.IsCertificatePending == null) &&
            EF.Functions.Like(w.SaleNumber, saleNumber + "%")
            )
                .Select(s => new KeyValuePair<int, string>(s.Id, s.SaleNumber)).ToListAsync();
            return allSaleNumbers;
        }

        public async Task<List<SaleProfitModel>> GetProfitByIdAsync(int saleId)
        {
            var result = new List<SaleProfitModel>();
            var query = gemStoContext.GemSales.Include(i => i.Gem).Where(f => f.Id == saleId && !f.IsDeleted && (f.GemId != null || f.Weight == null)).AsQueryable();
            try
            {
                var saleHeader = await query.FirstOrDefaultAsync();
                if (saleHeader.IsCertificatePending == null)
                {
                    // --------------------------- header row ------------------------------------------
                    result.Add(await query.Select(s => new SaleProfitModel
                    {
                        Id = s.Id,
                        SaleId = s.SaleNumber,
                        Variety = null,
                        Shape = null,
                        StockNo = null,
                        IsTreated = null,
                        NoOfPieces = s.NumberOfPieces,
                        Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                        Weight = null,
                        Date = s.Sale.CreatedOn,
                        SellingRate = s.SellingRate,
                        Price = s.TotalAmount,
                        TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                        PaymentStatus = s.PaymentStatus,
                        IsSingleSale = s.IsSingleSale,
                        IsCertificatePending = s.IsCertificatePending,
                        IsThirdParty = s.IsThirdParty,
                        Share = null,
                        IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                        BuyerId = s.BuyerId,
                        Commission = s.Sale.Commission,
                        VarietyId = null,
                        ShapeId = null,
                        GemId = null,
                        GemIdentity = Guid.Empty,
                        IsDeleted = s.IsDeleted,
                        GemTotalCost = null,
                        SaleIdGuid = s.SaleId
                    }).FirstOrDefaultAsync());

                    // ------------------------- end header row ----------------------------------------


                    // ------------------------- sold gems in lot ----------------------------------------


                    var querySoldGemsInLot = gemStoContext.GemSales.Include(i => i.Gem).Where(f => f.SaleId == saleHeader.SaleId && !f.IsDeleted && f.IsCertificatePending != null).AsQueryable();


                    var lot = await querySoldGemsInLot.Select(s => new SaleProfitModel
                    {
                        Id = s.Id,
                        SaleId = s.SaleNumber,
                        Variety = s.GemId != null ? s.Gem.Variety.Value : (s.GemId == null && !s.IsSingleSale ? null : s.Variety.Value),
                        Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : (s.GemId == null && !s.IsSingleSale ? null : s.Shape.ImagePath),
                        StockNo = s.GemId != null ? s.Gem.StockNumber : (s.GemId == null && !s.IsSingleSale ? null : s.ThirdPartyOwner),
                        IsTreated = s.GemId != null ? s.Gem.IsTreated : (s.GemId == null && !s.IsSingleSale ? null : s.IsTreated),
                        NoOfPieces = s.NumberOfPieces,
                        Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                        Weight = s.GemId != null ? (s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight) : (s.GemId == null && !s.IsSingleSale ? null : s.Weight),
                        Date = s.Sale.CreatedOn,
                        SellingRate = s.SellingRate,
                        Price = s.TotalAmount,
                        TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                        PaymentStatus = s.PaymentStatus,
                        IsSingleSale = s.IsSingleSale,
                        IsCertificatePending = s.IsCertificatePending,
                        IsThirdParty = s.IsThirdParty,
                        Share = s.GemId != null ? s.Gem.Share : 0.00m,
                        IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                        BuyerId = s.BuyerId,
                        Commission = s.Sale.Commission,
                        VarietyId = s.GemId != null ? s.Gem.VarietyId : s.VarietyId,
                        ShapeId = s.GemId != null ? s.Gem.ShapeId : s.ShapeId,
                        GemId = s.GemId.HasValue ? s.GemId : null,
                        GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty,
                        IsDeleted = s.IsDeleted,
                        GemTotalCost = s.Gem.TotalCost,
                        SaleIdGuid = s.SaleId
                    }).OrderByDescending(o => o.Id).ToListAsync();



                    // ------------------------- end sold gems in lot ----------------------------------------
                    var totalProfit = lot.Sum(s => ((s.SellingRate * s.Price) - s.GemTotalCost) * s.Share);

                    result.FirstOrDefault().LotProfit = totalProfit;
                    result.AddRange(lot);

                }
                else
                {
                    result.Add(await query.Select(s => new SaleProfitModel
                    {
                        Id = s.Id,
                        SaleId = s.SaleNumber,
                        Variety = s.GemId != null ? s.Gem.Variety.Value : (s.GemId == null && !s.IsSingleSale ? null : s.Variety.Value),
                        Shape = s.GemId != null ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : (s.GemId == null && !s.IsSingleSale ? null : s.Shape.ImagePath),
                        StockNo = s.GemId != null ? s.Gem.StockNumber : (s.GemId == null && !s.IsSingleSale ? null : s.ThirdPartyOwner),
                        IsTreated = s.GemId != null ? s.Gem.IsTreated : (s.GemId == null && !s.IsSingleSale ? null : s.IsTreated),
                        NoOfPieces = s.NumberOfPieces,
                        Buyer = s.BuyerId != null ? s.Buyer.Name : s.BuyerName,
                        Weight = s.GemId != null ? (s.Gem.RecutWeight != 0.00m ? s.Gem.RecutWeight : s.Gem.InitialWeight) : (s.GemId == null && !s.IsSingleSale ? null : s.Weight),
                        Date = s.Sale.CreatedOn,
                        SellingRate = s.SellingRate,
                        Price = s.TotalAmount,
                        TotalAmountReceived = s.TotalAmountPaid * s.SellingRate,
                        PaymentStatus = s.PaymentStatus,
                        IsSingleSale = s.IsSingleSale,
                        IsCertificatePending = s.IsCertificatePending,
                        IsThirdParty = s.IsThirdParty,
                        Share = s.GemId != null ? s.Gem.Share : 0.00m,
                        IsAllPaymentsApproved = !s.SalePayments.Any(a => !a.IsDeleted) ? false : s.SalePayments.Where(w => !w.IsDeleted).All(a => a.IsPaymentApproved),
                        BuyerId = s.BuyerId,
                        Commission = s.Sale.Commission,
                        VarietyId = s.GemId != null ? s.Gem.VarietyId : s.VarietyId,
                        ShapeId = s.GemId != null ? s.Gem.ShapeId : s.ShapeId,
                        GemId = s.GemId.HasValue ? s.GemId : null,
                        GemIdentity = s.GemId.HasValue ? s.Gem.Identity : Guid.Empty,
                        IsDeleted = s.IsDeleted,
                        GemTotalCost = s.Gem.TotalCost,
                        SaleIdGuid = s.SaleId
                    }).FirstOrDefaultAsync());
                }


                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteGemFromSale(int gemSaleId, string editedById, string editedByName)
        {
            using (var dbTransaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();
                var listOFGemStatusBeforeSale = new List<KeyValuePair<int, GemStatus>>();

                try
                {
                    var entity = await gemStoContext.GemSales.Include(i => i.GemExport).ThenInclude(i => i.Export).Include(i => i.Sale)
                        .Include(i => i.Gem)
                        .FirstOrDefaultAsync(f => f.Id == gemSaleId);

                    if (entity.IsSingleSale && !entity.IsDeleted)
                    {

                        if (entity.GemExportId != null && !entity.GemExport.Export.IsExportClosed)
                        {
                            if (entity.GemExport.IsThirdParty)
                            {
                                entity.GemExport.GemStatus = GemStatus.Exported;
                            }
                            else
                            {
                                listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>((int)entity.GemId, entity.Gem.GemStatus));
                                entity.Gem.GemStatus = GemStatus.Exported;
                            }
                        }

                        else if (!entity.IsThirdParty)
                        {
                            listOFGemStatusBeforeSale.Add(new KeyValuePair<int, GemStatus>((int)entity.GemId, entity.Gem.GemStatus));

                            entity.Gem.GemStatus = GemStatus.InStock;
                        }

                        if (!entity.IsThirdParty)
                        {
                            var gemStatusBeforeReturningFromSale = listOFGemStatusBeforeSale.FirstOrDefault(f => f.Key == entity.GemId);

                            gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.DeletedFromSales, entity.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number} as sale deleted", (int)entity.GemId, ActionEnum.Deleted, editedByName, gemStatusBeforeReturningFromSale.Value));
                        }
                    }
                    else if (!entity.IsSingleSale && !entity.IsDeleted)
                    {
                        var allSalGemsInLot = await gemStoContext.GemSales.Include(i => i.GemExport).ThenInclude(i => i.Export).Include(i => i.Gem).Include(i => i.Sale).Where(f => f.SaleId == entity.SaleId).ToListAsync();

                        if (entity.GemId == null)
                        {
                            entity.Sale.IsDeleted = true;
                            foreach (var saleItem in allSalGemsInLot)
                            {
                                var gemStatus = GemStatus.InStock;



                                if (saleItem.GemExportId != null && !saleItem.GemExport.Export.IsExportClosed)
                                {
                                    gemStatus = saleItem.Gem.GemStatus;
                                    saleItem.Gem.GemStatus = GemStatus.Exported;
                                }
                                else if (saleItem.GemId != null)
                                {
                                    gemStatus = saleItem.Gem.GemStatus;
                                    saleItem.Gem.GemStatus = GemStatus.InStock;
                                }

                                if (saleItem.GemId != null && !saleItem.IsDeleted)
                                {
                                    gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.DeletedFromSales, saleItem.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number} as sale deleted", (int)saleItem.GemId, ActionEnum.Deleted, editedByName, gemStatus));
                                }

                                saleItem.IsDeleted = true;
                            }
                        }

                        else
                        {
                            var saleGem = allSalGemsInLot.FirstOrDefault(f => f.Id == gemSaleId);

                            gemHistory.Add(new GemHistory().CreateSaleHistory(editedById, GemHistoryStatusEnum.DeletedFromSales, saleGem.Id, $"Removed from sale on Sale Id:- {entity.Sale.Number} as sale deleted", (int)saleGem.GemId, ActionEnum.Deleted, editedByName, saleGem.Gem.GemStatus));

                            saleGem.Gem.GemStatus = (saleGem.GemExportId != null && !saleGem.GemExport.Export.IsExportClosed) ? GemStatus.Exported : GemStatus.InStock;
                            var lotHeader = allSalGemsInLot.FirstOrDefault(f => f.GemId == null);
                            lotHeader.NumberOfPieces--;
                            lotHeader.TotalAmount -= saleGem.TotalAmount ?? 0.00m;

                            gemStoContext.GemHistory.AddRange(gemHistory);
                            await gemStoContext.SaveChangesAsync();

                            await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[GemSales]  WHERE Id={saleGem.Id}");
                            if (lotHeader.NumberOfPieces == 0)
                            {
                                await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[SalePayments]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[GemSales]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[Sales]  WHERE Id={entity.SaleId} ");
                            }

                            dbTransaction.Commit();
                            return;
                        }
                    }

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();

                    if (!entity.IsSingleSale && entity.IsDeleted && entity.GemId != null)
                    {
                        var allSalGemsInLot = await gemStoContext.GemSales.Include(i => i.GemExport).ThenInclude(i => i.Export).Include(i => i.Gem).Include(i => i.Sale).Where(f => f.SaleId == entity.SaleId).ToListAsync();
                        var lotHeader = allSalGemsInLot.FirstOrDefault(f => f.GemId == null);
                        lotHeader.NumberOfPieces--;
                        await gemStoContext.SaveChangesAsync();
                        await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[GemSales]  WHERE Id={entity.Id}");
                        if (lotHeader.NumberOfPieces == 0)
                        {
                            await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[SalePayments]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[GemSales]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[Sales]  WHERE Id={entity.SaleId} ");
                        }
                        return;
                    }
                    await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[SalePayments]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[GemSales]  WHERE SaleId={entity.SaleId} DELETE FROM [dbo].[Sales]  WHERE Id={entity.SaleId} ");

                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<SaleGemsWithCertificateModel>> GetSaleGemsWithCertifiate(string buyerName, bool isLocalCurrency)
        {
            try
            {
                var data = gemStoContext.GemSales.Include(i => i.Sale).ThenInclude(i => i.SalePayments).Include(i => i.Gem)
                     .Where(w => w.IsCertificatePending == false && w.PaymentStatus != PaymentStatus.Paid && !w.IsDeleted && (w.Buyer.Name == buyerName || w.BuyerName == buyerName))
                     .AsNoTracking()
                     .AsQueryable();


                var saleGemsWithCertificateModelList = new List<SaleGemsWithCertificateModel>();

                if (isLocalCurrency)
                {
                    saleGemsWithCertificateModelList = await data.Select(s => new SaleGemsWithCertificateModel
                    {
                        SaleNumber = s.SaleNumber,
                        StockNumber = s.GemId.HasValue ? s.Gem.StockNumber : s.ThirdPartyOwner,
                        Weight = s.GemId.HasValue ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : s.Weight,
                        Shape = s.GemId.HasValue ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,
                        SaleDate = s.CreatedOn.ToString("dd MMM yyyy"),
                        Variety = s.GemId.HasValue ? s.Gem.Variety.Value : s.Variety.Value,
                        Rate = s.SellingRate,
                        BuyerCurrency = s.TotalAmount ?? 0.00m,
                        AmountPaid = s.TotalAmountPaid != -1 ? (s.TotalAmountPaid * s.SellingRate) : (!s.Sale.SalePayments.Any() ? 0.00m : (s.Sale.SalePayments.Sum(c => c.AmmountPaid) * s.SellingRate) / s.Sale.GemSales.FirstOrDefault(f => f.GemId == null && !f.IsThirdParty).NumberOfPieces),
                        IsThirdParty = s.IsThirdParty
                    }).ToListAsync();
                }

                else
                {
                    saleGemsWithCertificateModelList = await data.Select(s => new SaleGemsWithCertificateModel
                    {
                        SaleNumber = s.SaleNumber,
                        StockNumber = s.GemId.HasValue ? s.Gem.StockNumber : s.ThirdPartyOwner,
                        Weight = s.GemId.HasValue ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : s.Weight,
                        Shape = s.GemId.HasValue ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,
                        SaleDate = s.CreatedOn.ToString("dd MMM yyyy"),
                        Variety = s.GemId.HasValue ? s.Gem.Variety.Value : s.Variety.Value,
                        Rate = s.SellingRate,
                        BuyerCurrency = s.TotalAmount ?? 0.00m,
                        AmountPaid = s.TotalAmountPaid != -1 ? s.TotalAmountPaid : (!s.Sale.SalePayments.Any() ? 0.00m : (s.Sale.SalePayments.Sum(c => c.AmmountPaid)) / s.Sale.GemSales.FirstOrDefault(f => f.GemId == null && !f.IsThirdParty).NumberOfPieces),
                        IsThirdParty = s.IsThirdParty
                    }).ToListAsync();
                }
                return saleGemsWithCertificateModelList;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<SaleGemsWithCertificatePendingModel>> GetSaleGemsWithCertificatePending(string buyerName, bool isLocalCurrency)
        {
            try
            {
                var data = gemStoContext.GemSales.Include(i => i.Sale).ThenInclude(i => i.SalePayments).Include(i => i.Gem)
                     .Where(w => w.IsCertificatePending == true && w.PaymentStatus != PaymentStatus.Paid && !w.IsDeleted && (w.Buyer.Name == buyerName || w.BuyerName == buyerName))
                     .AsNoTracking()
                     .AsQueryable();


                var saleGemsWithCertificatePendingModelList = new List<SaleGemsWithCertificatePendingModel>();

                if (isLocalCurrency)
                {
                    saleGemsWithCertificatePendingModelList = await data.Select(s => new SaleGemsWithCertificatePendingModel
                    {
                        SaleNumber = s.SaleNumber,
                        StockNumber = s.GemId.HasValue ? s.Gem.StockNumber : s.ThirdPartyOwner,
                        Weight = s.GemId.HasValue ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : s.Weight,
                        Shape = s.GemId.HasValue ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,
                        SaleDate = s.CreatedOn.ToString("dd MMM yyyy"),
                        Variety = s.GemId.HasValue ? s.Gem.Variety.Value : s.Variety.Value,
                        Rate = s.SellingRate,
                        BuyerCurrency = s.TotalAmount ?? 0.00m,
                        AmountPaid = s.TotalAmountPaid != -1 ? (s.TotalAmountPaid * s.SellingRate) : (!s.Sale.SalePayments.Any() ? 0.00m : (s.Sale.SalePayments.Sum(c => c.AmmountPaid) * s.SellingRate) / s.Sale.GemSales.FirstOrDefault(f => f.GemId == null && !f.IsThirdParty).NumberOfPieces),
                        IsThirdParty = s.IsThirdParty
                    }).ToListAsync();
                }

                else
                {
                    saleGemsWithCertificatePendingModelList = await data.Select(s => new SaleGemsWithCertificatePendingModel
                    {
                        SaleNumber = s.SaleNumber,
                        StockNumber = s.GemId.HasValue ? s.Gem.StockNumber : s.ThirdPartyOwner,
                        Weight = s.GemId.HasValue ? (s.Gem.RecutWeight == 0.00m ? s.Gem.InitialWeight : s.Gem.RecutWeight) : s.Weight,
                        Shape = s.GemId.HasValue ? (s.Gem.RecutShapeId != null ? s.Gem.RecutShape.ImagePath : s.Gem.Shape.ImagePath) : s.Shape.ImagePath,
                        SaleDate = s.CreatedOn.ToString("dd MMM yyyy"),
                        Variety = s.GemId.HasValue ? s.Gem.Variety.Value : s.Variety.Value,
                        Rate = s.SellingRate,
                        BuyerCurrency = s.TotalAmount ?? 0.00m,
                        AmountPaid = s.TotalAmountPaid != -1 ? s.TotalAmountPaid : (!s.Sale.SalePayments.Any() ? 0.00m : (s.Sale.SalePayments.Sum(c => c.AmmountPaid)) / s.Sale.GemSales.FirstOrDefault(f => f.GemId == null && !f.IsThirdParty).NumberOfPieces),
                        IsThirdParty = s.IsThirdParty
                    }).ToListAsync();
                }
                return saleGemsWithCertificatePendingModelList;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<KeyValuePair<int, string>>> GetAllSaleNumbersWithNoItemsinLotAsync(string saleNumber)
        {
            var allSaleNumbers = await gemStoContext.GemSales.Where(w => !w.IsDeleted && (w.IsSingleSale || w.IsThirdParty || w.GemId == null) &&
            EF.Functions.Like(w.SaleNumber, saleNumber + "%")
            )
                .Select(s => new KeyValuePair<int, string>(s.Id, s.SaleNumber)).ToListAsync();
            return allSaleNumbers;
        }
    }
}

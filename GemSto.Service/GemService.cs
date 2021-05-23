using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Domain.LookUp;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class GemService : IGemService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public GemService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<PaginationModel<GemModel>> GetAllAsync(PaginationBase paginationBase, bool isThirdParty)
        {
            try
            {
                var query = gemStoContext.Gems.Include(i => i.RecutShape)
                    .Where(w => !w.IsDeleted && w.IsThirdParty == isThirdParty).AsQueryable();

                if (paginationBase.SearchQuery != null && paginationBase.SearchQuery.Length > 0)
                {
                    if (paginationBase.IsAdmin)
                    {
                        query = query.Where(x => EF.Functions.Like(x.StockNumber, paginationBase.SearchQuery + "%") ||
                                                 (x.SellerId == null ? EF.Functions.Like(x.SellerName.ToLower(), paginationBase.SearchQuery + "%") :
                                                 EF.Functions.Like(x.Seller.Name.ToLower(), paginationBase.SearchQuery + "%")) ||
                                                   x.GemStatus == GetEnumValueByDescription.GetValueFromDescription<GemStatus>(paginationBase.SearchQuery));
                    }
                    else
                    {
                        query = query.Where(x => EF.Functions.Like(x.StockNumber, paginationBase.SearchQuery + "%"));
                    }
                }

                var totalNumberOfRecord = await query.AsNoTracking().CountAsync();

                query = query.OrderByDescending(x => x.Id).Skip(paginationBase.Skip).Take(paginationBase.Take);

                var result = query.Select(x => new GemModel
                {
                    Id = x.Id,
                    StockNumber = x.StockNumber,
                    Variety = new VarietyModel
                    {
                        Value = x.Variety.Value
                    },
                    InitialWeight = x.InitialWeight,
                    RecutWeight = x.RecutWeight,
                    IsTreated = x.IsTreated,
                    InitialCost = x.InitialCost,
                    TotalCost = x.TotalCost,
                    Shape = new ShapeModel
                    {
                        Value = x.RecutShapeId != null ? x.RecutShape.Value : x.Shape.Value,
                        ImagePath = x.RecutShapeId != null ? x.RecutShape.ImagePath : x.Shape.ImagePath
                    },
                    SellerName = x.SellerId != null ? x.Seller.Name : x.SellerName,
                    PaymentStatusToSeller = x.PaymentStatusToSeller,
                    TotalAmountPaidToSeller = x.TotalAmountPaidToSeller,
                    PurchasedDate = x.PurchasedDate.Value,
                    GemStatus = x.GemStatus, //will be automatically updated when inserting new gem or editing payment amount
                    NumberOfPieces = x.NumberOfPieces,
                    IsGemLot = x.IsGemLot,
                    Identity = x.Identity,
                    Share = x.Share,
                    SellingPrice = x.SellingPrice

                }).AsQueryable();

                var resultData = await result.AsNoTracking().ToListAsync();

                var resultSet = new PaginationModel<GemModel>()
                {
                    TotalRecords = totalNumberOfRecord,
                    Details = resultData
                };

                return resultSet;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<PaginationModel<GemModel>> GetBySearchQueryAsync(PaginationBase paginationBase, bool isThirdParty)
        {
            paginationBase.SearchQuery = paginationBase.SearchQuery.ToLower();

            var query = gemStoContext.Gems.Include(i => i.RecutShape).Where(w => !w.IsDeleted && w.IsThirdParty == isThirdParty);

            if (paginationBase.IsAdmin)
            {
                query = query.Where(x => EF.Functions.Like(x.StockNumber, paginationBase.SearchQuery + "%") ||
                                         (x.SellerId == null ? EF.Functions.Like(x.SellerName.ToLower(), paginationBase.SearchQuery + "%") :
                                         EF.Functions.Like(x.Seller.Name.ToLower(), paginationBase.SearchQuery + "%")) ||
                                           x.GemStatus == GetEnumValueByDescription.GetValueFromDescription<GemStatus>(paginationBase.SearchQuery));
            }
            else
            {
                query = query.Where(x => EF.Functions.Like(x.StockNumber, paginationBase.SearchQuery + "%"));
            }


            var totalNumberOfRecord = query.Count();

            query = query.OrderByDescending(x => x.Id).Skip(paginationBase.Skip).Take(paginationBase.Take);

            var result = query.Select(x => new GemModel
            {
                Id = x.Id,
                StockNumber = x.StockNumber,
                Variety = new VarietyModel
                {
                    Value = x.Variety.Value
                },
                InitialWeight = x.InitialWeight,
                RecutWeight = x.RecutWeight,
                IsTreated = x.IsTreated,
                InitialCost = x.InitialCost,
                TotalCost = x.TotalCost,
                Shape = new ShapeModel
                {
                    Value = x.RecutShapeId != null ? x.RecutShape.Value : x.Shape.Value,
                    ImagePath = x.RecutShapeId != null ? x.RecutShape.ImagePath : x.Shape.ImagePath
                },
                SellerName = x.SellerId != null ? x.Seller.Name : x.SellerName,
                PaymentStatusToSeller = x.PaymentStatusToSeller, //will be automatically updated when inserting new gem or editing payment amount
                TotalAmountPaidToSeller = x.TotalAmountPaidToSeller,
                PurchasedDate = x.PurchasedDate,
                GemStatus = x.GemStatus, //will be automatically updated when inserting new gem or editing payment amount
                NumberOfPieces = x.NumberOfPieces,
                Identity = x.Identity,
                IsGemLot = x.IsGemLot,
                Share = x.Share
            }).AsQueryable();

            var resultData = await result.AsNoTracking().ToListAsync();

            var resultSet = new PaginationModel<GemModel>()
            {
                TotalRecords = totalNumberOfRecord,
                Details = resultData
            };
            return resultSet;
        }

        public async Task CreateNewGemAsync(SignleGemCreateModel gemModel)
        {
            try
            {
                var paymentStatus = PaymentStatus.Unpaid;

                if (gemModel.Share == 1.00m)
                {
                    paymentStatus = gemModel.TotalAmountPaidToSeller == 0 ?
                                            PaymentStatus.Unpaid : (gemModel.TotalAmountPaidToSeller < (gemModel.InitialCost + (gemModel.BrokerFee ?? 0)) ? PaymentStatus.Partial : PaymentStatus.Paid);
                }
                else
                {
                    var payable = (gemModel.InitialCost + (gemModel.BrokerFee ?? 0)) * gemModel.Share;

                    paymentStatus = gemModel.TotalAmountPaidToSeller == 0 ?
                                            PaymentStatus.Unpaid : (gemModel.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                }

                var entity = new Gem
                {
                    InitialCost = gemModel.InitialCost,
                    SellerName = gemModel.SellerName,
                    SellerId = gemModel.Seller?.Id,
                    InitialWeight = gemModel.InitialWeight,
                    IsTreated = gemModel.IsTreated,
                    ShapeId = gemModel.Shape.Id,
                    StockNumber = gemModel.StockNumber,
                    TotalAmountPaidToSeller = gemModel.TotalAmountPaidToSeller,
                    VarietyId = gemModel.Variety.Id,
                    GemStatus = GemStatus.InStock,
                    TotalCost = gemModel.InitialCost + (gemModel.BrokerFee ?? 0),
                    NumberOfPieces = 1,
                    PaymentStatusToSeller = paymentStatus,
                    Length = gemModel.Length ?? 0,
                    Width = gemModel.Width ?? 0,
                    Depth = gemModel.Depth ?? 0,
                    BrokerFee = gemModel.BrokerFee ?? 0,
                    CreatedById = gemModel.CreatedById,
                    PurchasedDate = gemModel.PurchasedDate,
                    Note = gemModel.Note,
                    Share = gemModel.Share
                };

                var savedEntity = gemStoContext.Gems.Add(entity);

                var sellerName = gemModel.SellerName;
                if (sellerName is null)
                {
                    sellerName = await gemStoContext.Sellers.Where(w => w.Id == (int)gemModel.Seller.Id).Select(s => s.Name).FirstOrDefaultAsync();
                }

                await gemStoContext.SaveChangesAsync();

                var gemPurchasedHistory = new GemHistory().CreateSingleGemPurchased(savedEntity.Entity.Id, sellerName, gemModel.CreatedByName, gemModel.CreatedById, entity.GemStatus);

                gemStoContext.GemHistory.Add(gemPurchasedHistory);
                await gemStoContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.StartsWith("Cannot insert duplicate key row in object"))
                    throw new Exception($"Stock number {gemModel.StockNumber} has been taken, save again with the updated stock number");
                else
                    throw new Exception(ex.Message);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteSingleGemAsync(int gemIdToDelete, string deletedByName, string createdById)
        {
            var gemHistorylist = new List<GemHistory>();
            var isGemToReturn = await gemStoContext.Gems
                 .Include(i => i.Transactions).Include(i => i.Certificates).FirstOrDefaultAsync(a => a.Id == gemIdToDelete);

            if (isGemToReturn.VarietyId == null && isGemToReturn.ShapeId == null)
            {
                var gemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == isGemToReturn.GemLotId).ToListAsync();
                var isAllInStock = gemsInLot.Where(w => w.IsTreated != null).All(a => a.GemStatus == GemStatus.InStock);

                if (!isAllInStock)
                {
                    throw new Exception("All gems must be in stock to delete lot");
                }
                else
                {
                    var gLot = gemsInLot.FirstOrDefault(w => w.IsTreated == null);
                    gemHistorylist.Add(new GemHistory().DeleteGem(gLot.Id, deletedByName, createdById, true, gLot.StockNumber, gLot.GemStatus));
                    foreach (var gem in gemsInLot)
                    {
                        var item = gem.Id;

#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                        await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[Transactions] WHERE GemId={item} " +
                                                                    $"DELETE FROM [dbo].[GemSales] WHERE GemId={item}" +
                                                                    $"DELETE FROM [dbo].[Certifications] WHERE GemId={item}" +
                                                                    $"DELETE FROM [dbo].[Certificates] WHERE GemId={item}" +
                                                                    $"DELETE FROM [dbo].[Gems] WHERE Id={item}");
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                    }
                }
            }

            else
            {

                if (isGemToReturn.IsGemLot)
                {
                    var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.GemLotId == isGemToReturn.GemLotId && f.IsTreated == null);
                    gemLotHeader.InitialCost -= isGemToReturn.InitialCost;
                    gemLotHeader.TotalCost -= isGemToReturn.TotalCost;
                    --gemLotHeader.NumberOfPieces;

                    var paymentStatus = PaymentStatus.Unpaid;

                    if (gemLotHeader.Share == 1.00m)
                    {
                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    else
                    {
                        var payable = (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) * gemLotHeader.Share;

                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    gemLotHeader.PaymentStatusToSeller = paymentStatus;

                    var allGemsinlotWithheader = await gemStoContext.Gems.Where(w => w.GemLotId == gemLotHeader.GemLotId).ToListAsync();
                    allGemsinlotWithheader.ForEach(f => f.PaymentStatusToSeller = paymentStatus);
                    //await gemStoContext.SaveChangesAsync();
                }
#pragma warning disable EF1000 // Possible SQL injection vulnerability.

                gemHistorylist.Add(new GemHistory().DeleteGem(isGemToReturn.Id, deletedByName, createdById, false, isGemToReturn.StockNumber, isGemToReturn.GemStatus));

                await gemStoContext.Database.ExecuteSqlCommandAsync($"DELETE FROM [dbo].[Transactions] WHERE GemId={gemIdToDelete} " +
                                                                    $"DELETE FROM [dbo].[GemSales] WHERE GemId={gemIdToDelete}" +
                                                                    $"DELETE FROM [dbo].[Certifications] WHERE GemId={gemIdToDelete}" +
                                                                    $"DELETE FROM [dbo].[Certificates] WHERE GemId={gemIdToDelete}" +
                                                                    $"DELETE FROM [dbo].[Gems] WHERE Id={gemIdToDelete}");
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            }

            gemStoContext.GemHistory.AddRange(gemHistorylist);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<string> LastStockIdAsync(bool isThirdParty)
        {
            var entity = gemStoContext.Gems
                .Where(w => (!w.IsGemLot || w.IsTreated == null) && w.IsThirdParty == isThirdParty).OrderBy(o => o.Id);
            var result = await entity.LastOrDefaultAsync();


            return result?.StockNumber ?? string.Empty;
        }

        public async Task SaveGemLotAsync(GemLotListCreateModel gemLotListCreateModel)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                var gemHistory = new List<GemHistory>();
                try
                {
                    var list = new List<Gem>();

                    var paymentStatus = PaymentStatus.Unpaid;

                    if (gemLotListCreateModel.Share == 1.00m)
                    {
                        paymentStatus = gemLotListCreateModel.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotListCreateModel.TotalAmountPaidToSeller < (gemLotListCreateModel.GemLotCost + (gemLotListCreateModel.BrokerFee ?? 0)) ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    else
                    {
                        var payable = (gemLotListCreateModel.GemLotCost + (gemLotListCreateModel.BrokerFee ?? 0)) * gemLotListCreateModel.Share;

                        paymentStatus = gemLotListCreateModel.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotListCreateModel.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }

                    var lotSummary = new Gem()
                    {
                        StockNumber = gemLotListCreateModel.StockNumber,
                        VarietyId = null,
                        InitialWeight = gemLotListCreateModel.LotList.Sum(s => s.InitialWeight),
                        RecutWeight = 0,
                        IsTreated = null,
                        InitialCost = gemLotListCreateModel.GemLotCost,
                        TotalCost = gemLotListCreateModel.GemLotCost + (gemLotListCreateModel.BrokerFee ?? 0),
                        ShapeId = null,
                        SellerId = gemLotListCreateModel.Seller?.Id,
                        GemStatus = GemStatus.GemLot,
                        SellerName = gemLotListCreateModel.SellerName,
                        NumberOfPieces = gemLotListCreateModel.LotList.Count(),
                        PaymentStatusToSeller = paymentStatus,
                        TotalAmountPaidToSeller = gemLotListCreateModel.TotalAmountPaidToSeller,
                        BrokerFee = gemLotListCreateModel.BrokerFee ?? 0,
                        IsGemLot = true,
                        CreatedById = gemLotListCreateModel.CreatedById,
                        Note = gemLotListCreateModel.Note,
                        PurchasedDate = gemLotListCreateModel.PurchasedDate,
                        RecutShapeId = null,
                        Share = gemLotListCreateModel.Share
                    };

                    list.Add(lotSummary);

                    var counter = 1;

                    var gemLot = new GemLot
                    {
                        TotalAmountPaidToSeller = gemLotListCreateModel.TotalAmountPaidToSeller,
                        TotalCost = gemLotListCreateModel.GemLotCost,
                        Gems = list,
                        PaymentStatusToSeller = paymentStatus,

                        BrokerFee = gemLotListCreateModel.BrokerFee ?? 0
                    };

                    foreach (var gem in gemLotListCreateModel.LotList)
                    {
                        list.Add(new Gem
                        {
                            StockNumber = gemLotListCreateModel.StockNumber + "-" + counter++.ToString(),
                            InitialCost = gem.InitialCost,
                            InitialWeight = gem.InitialWeight,
                            IsTreated = gem.IsTreated,
                            ShapeId = gem.Shape.Id,
                            VarietyId = gem.Variety.Id,
                            TotalCost = gem.InitialCost,
                            GemStatus = GemStatus.InStock,
                            NumberOfPieces = 1,
                            SellerName = gemLotListCreateModel.SellerName,
                            SellerId = gemLotListCreateModel.Seller?.Id,
                            GemLotId = gemLot.Id,
                            Length = gem.Length ?? 0,
                            Width = gem.Width ?? 0,
                            Depth = gem.Depth ?? 0,
                            IsGemLot = true,
                            Share = gemLotListCreateModel.Share,
                            PaymentStatusToSeller = lotSummary.PaymentStatusToSeller
                        });
                    }

                    list.Reverse();

                    await gemStoContext.GemLots.AddAsync(gemLot);
                    await gemStoContext.SaveChangesAsync();

                    var sellerName = gemLotListCreateModel.SellerName;
                    if (sellerName is null)
                    {
                        sellerName = await gemStoContext.Sellers.Where(w => w.Id == (int)gemLotListCreateModel.Seller.Id).Select(s => s.Name).FirstOrDefaultAsync();
                    }
                    foreach (var gem in gemLot.Gems)
                    {
                        gemHistory.Add(new GemHistory().CreateSingleGemPurchased(gem.Id, sellerName, gemLotListCreateModel.CreatedByName, gemLotListCreateModel.CreatedById, gem.GemStatus));
                    }

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();

                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    if (ex.InnerException.Message.StartsWith("Cannot insert duplicate key row in object"))
                        throw new Exception($"Stock number {gemLotListCreateModel.StockNumber} has been taken, save again with the updated stock number");
                    else
                        throw new Exception(ex.Message);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

                transaction.Commit();
            }
        }

        public async Task<GemModel> GetByIdAsync(Guid gemIdentity)
        {
            try
            {
                var query = gemStoContext.Gems
                    .Include(i => i.Shape).Include(i => i.RecutShape).Include(i => i.Certificates).Include(i => i.Variety).Include(i => i.Seller).Include(i => i.Transactions)
               .Where(w => w.Identity == gemIdentity && !w.IsDeleted).AsQueryable();

                var entity = await query.FirstOrDefaultAsync();

                var model = mapper.Map<GemModel>(entity);
                if (model.SellerName != null)
                    model.Seller = null;

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateGemDetailAsync(GemUpdateModel gemUpdateModel)
        {
            var gemHistory = new List<GemHistory>();
            try
            {
                using (gemStoContext)
                {
                    var entity = await gemStoContext.Gems.Include(i => i.GemExports).FirstOrDefaultAsync(f => f.Id == gemUpdateModel.Id);

                    if (entity.Note != gemUpdateModel.Note)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Note edited"));
                    }
                    entity.Note = gemUpdateModel.Note;

                    if (entity.PurchasedDate != gemUpdateModel.PurchasedDate)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Received" : "Purchase")} date editd from {entity.PurchasedDate.Value.LocalDateTime.ToShortDateString()} to {gemUpdateModel.PurchasedDate.Value.LocalDateTime.ToShortDateString()}"));
                    }
                    entity.PurchasedDate = gemUpdateModel.PurchasedDate;

                    var allTransaction = await gemStoContext.Transactions
                           .Where(f => f.GemId == gemUpdateModel.Id).ToListAsync();

                    if (entity.InitialCost != gemUpdateModel.InitialCost)
                    {

                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Price" : "Initial cost")} editd from {entity.InitialCost} to {gemUpdateModel.InitialCost}"));

                        var gemPurchaseTransaction = allTransaction
                            .FirstOrDefault(f => f.GemId == gemUpdateModel.Id && f.TransactionType == TransactionType.GemPurchase);

                        gemPurchaseTransaction.PaidAmount = gemUpdateModel.InitialCost;

                        gemPurchaseTransaction.EditedById = gemUpdateModel.EditedById;
                        gemPurchaseTransaction.Remark = Remark.Edited;
                        gemPurchaseTransaction.EditedOn = DateTimeOffset.UtcNow;
                        entity.TotalCost = entity.TotalCost - entity.InitialCost + gemUpdateModel.InitialCost;

                        gemStoContext.Entry(gemPurchaseTransaction).State = EntityState.Modified;
                    }

                    if (entity.IsGemLot && entity.IsTreated != null)
                    {
                        var headerStockNumber = entity.StockNumber.Contains("TP") ? "TP-" + (entity.StockNumber.Split('-')[1]) : entity.StockNumber.Split('-')[0];

                        var gemLotEnity = await gemStoContext.Gems
                            .Where(f => f.StockNumber.StartsWith(headerStockNumber) && !f.IsDeleted && f.Id != gemUpdateModel.Id && f.GemStatus != GemStatus.Returned).ToListAsync();

                        var gemLotHeader = gemLotEnity.FirstOrDefault(f => f.StockNumber == headerStockNumber);

                        if (gemLotHeader.InitialCost < (gemUpdateModel.InitialCost + gemLotEnity
                            .Where(w => w.IsTreated != null).Sum(s => s.InitialCost)))
                        {
                            throw new Exception("Gem lot cost has been exceeded");
                        }

                        if (entity.InitialWeight != gemUpdateModel.InitialWeight)
                        {
                            gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Initial weight editd from {entity.InitialWeight} to {gemUpdateModel.InitialWeight}"));

                            gemLotHeader.InitialWeight = gemLotHeader.InitialWeight - entity.InitialWeight + gemUpdateModel.InitialWeight;
                        }

                        gemStoContext.Entry(gemLotHeader).State = EntityState.Modified;

                    }

                    if (entity.BrokerFee != gemUpdateModel.BrokerFee)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Broker fee editd from {entity.BrokerFee} to {gemUpdateModel.BrokerFee ?? 0.00m}"));

                        if (!entity.IsGemLot || entity.IsTreated == null)
                        {
                            var brokerTransaction = allTransaction.FirstOrDefault(f => f.TransactionType == TransactionType.BrokerPayment);
                            brokerTransaction.PaidAmount = gemUpdateModel.BrokerFee ?? 0;

                            brokerTransaction.Remark = Remark.Edited;
                            brokerTransaction.EditedOn = DateTimeOffset.UtcNow;
                            brokerTransaction.EditedById = gemUpdateModel.EditedById;

                            gemStoContext.Entry(brokerTransaction).State = EntityState.Modified;


                            entity.TotalCost = entity.TotalCost - entity.BrokerFee + (gemUpdateModel.BrokerFee ?? 0);
                            entity.BrokerFee = gemUpdateModel.BrokerFee ?? 0;

                        }
                    }

                    entity.InitialCost = gemUpdateModel.InitialCost;

                    if (gemUpdateModel.SellerName != null)
                    {
                        if (entity.IsTreated == null)
                        {
                            var allGemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == entity.GemLotId).ToListAsync();

                            foreach (var gem in allGemsInLot)
                            {
                                if (gem.SellerName != gemUpdateModel.SellerName)
                                {
                                    gemHistory.Add(new GemHistory().EditGemDetail(gem.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Owner" : "Seller")} name changed to {gemUpdateModel.SellerName}"));
                                }

                                if (gem.Share != gemUpdateModel.Share)
                                {
                                    gemHistory.Add(new GemHistory().EditGemDetail(gem.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Share edited from {entity.Share } to {gemUpdateModel.Share}"));
                                }

                                gem.SellerName = gemUpdateModel.SellerName;
                                gem.SellerId = null;
                                gem.Share = gemUpdateModel.Share;
                            }
                        }

                        else
                        {
                            if (entity.SellerName != gemUpdateModel.SellerName)
                            {
                                gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Owner" : "Seller")} name changed to {gemUpdateModel.SellerName}"));
                            }

                            entity.SellerName = gemUpdateModel.SellerName;
                            entity.SellerId = null;
                        }

                    }

                    else if (gemUpdateModel.Seller != null)
                    {
                        var seller = await gemStoContext.Sellers.Where(w => w.Id == gemUpdateModel.Seller.Id).Select(s => s.Name).FirstOrDefaultAsync();

                        if (entity.IsTreated == null)
                        {
                            var allGemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == entity.GemLotId).ToListAsync();



                            foreach (var gem in allGemsInLot)
                            {
                                if (entity.SellerId != gemUpdateModel.Seller.Id)
                                {
                                    gemHistory.Add(new GemHistory().EditGemDetail(gem.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Owner" : "Seller")}name changed to {seller}"));
                                }

                                if (gem.Share != gemUpdateModel.Share)
                                {
                                    gemHistory.Add(new GemHistory().EditGemDetail(gem.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Share edited from {entity.Share } to {gemUpdateModel.Share}"));
                                }
                                gem.SellerName = null;
                                gem.SellerId = gemUpdateModel.Seller.Id;
                                gem.Share = gemUpdateModel.Share;
                            }
                        }

                        else
                        {
                            if (entity.SellerId != gemUpdateModel.Seller.Id)
                            {
                                gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"{(entity.IsThirdParty ? "Owner" : "Seller")} name changed to {seller}"));
                            }

                            entity.SellerName = null;
                            entity.SellerId = gemUpdateModel.Seller.Id;
                        }
                    }

                    entity.SellerId = gemUpdateModel.Seller?.Id;


                    if (entity.InitialWeight != gemUpdateModel.InitialWeight)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Initial weight edited from {entity.InitialWeight} to {gemUpdateModel.InitialWeight}"));
                    }
                    entity.InitialWeight = gemUpdateModel.InitialWeight;

                    if (entity.IsTreated != null && gemUpdateModel.IsTreated != null)
                    {
                        if (entity.IsTreated != gemUpdateModel.IsTreated)
                        {
                            var oldTreatment = (bool)entity.IsTreated ? "heated" : "natural";
                            var newTreatment = (bool)gemUpdateModel.IsTreated ? "heated" : "natural";

                            gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Treatment edited from {oldTreatment} to {newTreatment}"));

                        }
                    }

                    entity.IsTreated = gemUpdateModel.IsTreated;

                    if (gemUpdateModel.IsTreated != null)
                    {
                        if (entity.ShapeId != gemUpdateModel.Shape.Id)
                        {

                            var oldShape = gemStoContext.Shapes.Where(w => w.Id == entity.ShapeId).Select(s => s.Value).FirstOrDefault();
                            var newShape = gemStoContext.Shapes.Where(w => w.Id == gemUpdateModel.Shape.Id).Select(s => s.Value).FirstOrDefault();


                            gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Shape edited from {oldShape} to {newShape}"));
                        }
                        entity.ShapeId = gemUpdateModel.Shape.Id;

                        var localRecutshape = gemUpdateModel.RecutShape.Id == 0 ? (int?)null : gemUpdateModel.RecutShape.Id;
                        if (entity.RecutShapeId != localRecutshape)
                        {
                            var oldRcutShape = gemStoContext.Shapes.Where(w => w.Id == entity.RecutShapeId).Select(s => s.Value).FirstOrDefault();
                            var newRecutShape = gemStoContext.Shapes.Where(w => w.Id == gemUpdateModel.RecutShape.Id).Select(s => s.Value).FirstOrDefault();

                            oldRcutShape = oldRcutShape ?? "no recut shape";
                            newRecutShape = newRecutShape ?? "no recut shape";

                            if (gemUpdateModel.RecutShape.Id == 0)
                            {
                                gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Recut shape edited from {oldRcutShape} to {newRecutShape}"));
                            }

                            else
                            {
                                gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Recut shape edited from {oldRcutShape} to {newRecutShape}"));
                            }
                        }
                        entity.RecutShapeId = gemUpdateModel.RecutShape.Id == 0 ? (int?)null : gemUpdateModel.RecutShape.Id;

                        if (entity.VarietyId != gemUpdateModel.Variety.Id)
                        {
                            var oldVariety = gemStoContext.Varieties.Where(w => w.Id == entity.VarietyId).Select(s => $"{s.Value}-{s.Description}").FirstOrDefault();
                            var newVariety = gemStoContext.Varieties.Where(w => w.Id == gemUpdateModel.Variety.Id).Select(s => $"{s.Value}-{s.Description}").FirstOrDefault();

                            gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Variety edited from {oldVariety} to {newVariety}"));
                        }
                        entity.VarietyId = gemUpdateModel.Variety.Id;
                    }

                    else
                    {
                        entity.ShapeId = null;
                        entity.RecutShapeId = null;
                        entity.VarietyId = null;
                    }

                    if (entity.Share != gemUpdateModel.Share)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Share edited from {entity.Share } to {gemUpdateModel.Share}"));
                    }
                    entity.Share = gemUpdateModel.Share;



                    if (!entity.IsGemLot || entity.IsTreated == null)
                    {
                        var paymentStatus = PaymentStatus.Unpaid;
                        if (entity.Share == 1.00m)
                        {
                            paymentStatus = entity.TotalAmountPaidToSeller == 0 ?
                            PaymentStatus.Unpaid : (entity.TotalAmountPaidToSeller < (entity.InitialCost + entity.BrokerFee) ?
                            PaymentStatus.Partial : PaymentStatus.Paid);
                        }

                        else
                        {
                            var payable = (entity.InitialCost + entity.BrokerFee) * entity.Share;

                            paymentStatus = entity.TotalAmountPaidToSeller == 0 ? PaymentStatus.Unpaid : (entity.TotalAmountPaidToSeller < payable ?
                                            PaymentStatus.Partial : PaymentStatus.Paid);
                        }

                        entity.PaymentStatusToSeller = paymentStatus;

                        if (entity.IsTreated == null)
                        {
                            var allGemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == entity.GemLotId).ToListAsync();
                            allGemsInLot.ForEach(f => f.PaymentStatusToSeller = entity.PaymentStatusToSeller);
                        }
                    }

                    if (entity.Length != gemUpdateModel.Length)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Length edited from {entity.Length} to {gemUpdateModel.Length ?? 0.00m}"));

                    }
                    entity.Length = gemUpdateModel.Length ?? 0;

                    if (entity.Width != gemUpdateModel.Width)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Width edited from {entity.Width} to {gemUpdateModel.Width ?? 0.00m}"));

                    }
                    entity.Width = gemUpdateModel.Width ?? 0;

                    if (entity.Depth != gemUpdateModel.Depth)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Depth edited from {entity.Depth} to {gemUpdateModel.Depth ?? 0.00m}"));

                    }
                    entity.Depth = gemUpdateModel.Depth ?? 0;

                    if (entity.RecutWeight != gemUpdateModel.RecutWeight)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Recut weight edited from {entity.RecutWeight} to {gemUpdateModel.RecutWeight}"));

                    }
                    entity.RecutWeight = gemUpdateModel.RecutWeight;

                    if (gemUpdateModel.IsLost && entity.GemStatus != GemStatus.Lost)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Gem marked as lost"));
                    }

                    if (!gemUpdateModel.IsLost && entity.GemStatus == GemStatus.Lost)
                    {
                        gemHistory.Add(new GemHistory().EditGemDetail(entity.Id, gemUpdateModel.CreatedByName, gemUpdateModel.EditedById, entity.GemStatus, $"Gem marked as not lost"));
                    }

                    if (gemUpdateModel.IsLost)
                    {
                        entity.GemStatus = GemStatus.Lost;
                        var exported = entity.GemExports.FirstOrDefault(f => f.GemId == gemUpdateModel.Id && !f.IsDeleted);
                        if (exported != null)
                        {
                            exported.IsDeleted = true;
                        }

                    }

                    else if (!gemUpdateModel.IsLost && entity.GemStatus == GemStatus.Lost)
                    {
                        entity.GemStatus = GemStatus.InStock;
                    }

                    entity.EditedById = gemUpdateModel.EditedById;
                    entity.EditedOn = DateTimeOffset.UtcNow;
                    gemStoContext.GemHistory.AddRange(gemHistory);
                    gemStoContext.Entry(entity).State = EntityState.Modified;
                    await gemStoContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task ReturnSingleGemAsync(int gemIdToReturn, string EditedById, string editedByName)
        {
            var gemHistorylist = new List<GemHistory>();

            var isGemToReturn = await gemStoContext.Gems
                .Include(i => i.Transactions).Include(i => i.Certificates).FirstOrDefaultAsync(a => a.Id == gemIdToReturn);

            if (isGemToReturn.VarietyId == null && isGemToReturn.ShapeId == null)
            {

                var canReturnAll = await gemStoContext.Gems.Where(w => w.GemLotId == isGemToReturn.GemLotId && w.GemStatus != GemStatus.GemLot).AllAsync(a => a.GemStatus == GemStatus.InStock || a.GemStatus == GemStatus.Returned || a.GemStatus == GemStatus.Lost);

                if (!canReturnAll)
                {
                    throw new Exception("All gems must be in stock, returned or lost to return this lot");
                }

                else
                {

                    gemHistorylist.Add(new GemHistory().ReturnGem(gemIdToReturn, editedByName, EditedById, GemStatus.GemLot, true, string.Empty, ActionEnum.Removed));


                    var allGemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == isGemToReturn.GemLotId && w.IsTreated.HasValue && w.GemStatus != GemStatus.Returned).ToListAsync();

                    allGemsInLot.ForEach(f => { gemHistorylist.Add(new GemHistory().ReturnGem(f.Id, editedByName, EditedById, f.GemStatus, false, f.StockNumber, ActionEnum.Removed)); });

                    await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Gems] SET GemStatus = {(int)GemStatus.Returned}, EditedById={EditedById} WHERE GemLotId={isGemToReturn.GemLotId} UPDATE [dbo].[GemExports] SET IsDeleted = 1 WHERE GemId IN (SELECT g.Id FROM [dbo].[Gems] g INNER JOIN [dbo].[GemExports] ge ON ge.GemId = g.Id WHERE g.GemLotId = {isGemToReturn.GemLotId})");
                }
            }

            else
            {
                await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Gems] SET GemStatus = {(int)GemStatus.Returned},EditedById={EditedById} WHERE Id={gemIdToReturn}");

                gemHistorylist.Add(new GemHistory().ReturnGem(gemIdToReturn, editedByName, EditedById, isGemToReturn.GemStatus, false, null, ActionEnum.Removed));

                if (isGemToReturn.IsGemLot)
                {
                    var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.GemLotId == isGemToReturn.GemLotId && f.IsTreated == null);


                    gemHistorylist.Add(new GemHistory().ReturnGem(gemLotHeader.Id, editedByName, EditedById, isGemToReturn.GemStatus, true, isGemToReturn.StockNumber, ActionEnum.Edited));


                    gemLotHeader.InitialCost -= isGemToReturn.InitialCost;
                    gemLotHeader.TotalCost -= isGemToReturn.TotalCost;

                    var paymentStatus = PaymentStatus.Unpaid;

                    if (gemLotHeader.Share == 1.00m)
                    {
                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    else
                    {
                        var payable = (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) * gemLotHeader.Share;

                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    gemLotHeader.PaymentStatusToSeller = paymentStatus;

                    var allGemsinlotWithheader = await gemStoContext.Gems.Where(w => w.GemLotId == gemLotHeader.GemLotId).ToListAsync();
                    allGemsinlotWithheader.ForEach(f => f.PaymentStatusToSeller = paymentStatus);

                }
            }
            gemStoContext.GemHistory.AddRange(gemHistorylist);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Object>> GetGemStockIdsAsync(string stockId, GemStatus gemStatus, bool? isThirdParty)
        {
            var query = gemStoContext.Gems.Where(w => !w.IsDeleted);

            if (isThirdParty.HasValue)
            {
                query = query.Where(w => w.IsThirdParty == isThirdParty.Value);
            }

            if (GemStatus.InStock == gemStatus)
            {
                var stockNumbers = await query.Where(w => w.GemStatus == GemStatus.InStock &&
             EF.Functions.Like(w.StockNumber, stockId + "%"))
                .Select(s => new { stockId = s.StockNumber, s.Id }).AsNoTracking().ToListAsync();

                return stockNumbers;
            }

            else if (GemStatus.Sold == gemStatus || GemStatus.SoldCP == gemStatus)
            {
                var stockNumbers = await query.Where(w => (w.GemStatus == GemStatus.Sold || w.GemStatus == GemStatus.SoldCP) &&
             EF.Functions.Like(w.StockNumber, stockId + "%"))
                .Select(s => new { stockId = s.StockNumber, s.Id }).AsNoTracking().ToListAsync();

                return stockNumbers;
            }

            else if (GemStatus.Certification == gemStatus)
            {
                var stockNumbers = await query.Where(w =>
                (w.GemStatus == GemStatus.Sold || w.GemStatus == GemStatus.InStock) &&
                EF.Functions.Like(w.StockNumber, stockId + "%"))
               .Select(s => new { stockId = s.StockNumber, gemStatus = s.GemStatus.DescriptionAttr(), s.Id }).AsNoTracking().ToListAsync();
                return stockNumbers;
            }

            else if (GemStatus.SingleAndLot == gemStatus)
            {
                var stockNumbers = await query.Where(w => w.GemStatus != GemStatus.Returned &&
                w.GemStatus != GemStatus.Lost &&
                w.GemStatus != GemStatus.Sold &&
                w.GemStatus != GemStatus.SoldCP &&
                //w.GemStatus != GemStatus.GemLot
                //&&
                EF.Functions.Like(w.StockNumber, stockId + "%"))
               .Select(s => new { stockId = s.StockNumber, gemStatus = s.GemStatus.DescriptionAttr(), s.Id }).AsNoTracking().ToListAsync();
                return stockNumbers;
            }
            else
            {
                var stockNumbers = await query.Where(w => w.GemStatus != GemStatus.Returned &&
                w.GemStatus != GemStatus.Lost &&
                w.GemStatus != GemStatus.Sold &&
                w.GemStatus != GemStatus.SoldCP &&
                w.GemStatus != GemStatus.GemLot
                &&
                EF.Functions.Like(w.StockNumber, stockId + "%"))
               .Select(s => new { stockId = s.StockNumber, gemStatus = s.GemStatus.DescriptionAttr(), s.Id }).AsNoTracking().ToListAsync();
                return stockNumbers;
            }
        }

        public async Task<IEnumerable<Object>> GetGemStockIdsForPayentLogAsync(string stockId)
        {
            var query = gemStoContext.Transactions
                                     .Where(w => !w.IsDeleted && w.Remark != Remark.Deleted && w.Gem.GemStatus != GemStatus.Returned &&
                                            w.TransactionType == TransactionType.SellerPayment &&
                                            w.PaidAmount != 0.00m
                                           );


            var stockNumbers = await query.Where(w => EF.Functions.Like(w.Gem.StockNumber, stockId + "%"))
            .Select(s => new { stockId = s.Gem.StockNumber, s.Gem.Id }).Distinct().AsNoTracking().ToListAsync();

            return stockNumbers;
        }

        public async Task GetGemByIdWithFilterAsync(Guid gemId, string filterBy)
        {
            var query = gemStoContext.Gems.Where(w => w.Identity == gemId && !w.IsDeleted).AsQueryable();

            if (filterBy == "DefaultCertificate")
            {
                query = query.Where(w => w.Certificates.Any(a => a.IsDefault));
            }

            var entity = await query.FirstOrDefaultAsync();
        }

        public async Task UpdateGemStatusAsync(int gemId, GemStatus gemStatus, string editedById, string editedByName)
        {
            var gemHistorylist = new List<GemHistory>();


            var isGemToUpdate = await gemStoContext.Gems
                .Include(i => i.Transactions).Include(i => i.Certificates).FirstOrDefaultAsync(a => a.Id == gemId);

            if (isGemToUpdate.VarietyId == null && isGemToUpdate.ShapeId == null)
            {
                gemHistorylist.Add(new GemHistory().UpdateGemStatus(gemId, editedByName, editedById, isGemToUpdate.GemStatus, GemStatus.GemLot, true, null, ActionEnum.Edited));

                var allGemsInLot = await gemStoContext.Gems.Where(w => w.GemLotId == isGemToUpdate.GemLotId && w.IsTreated.HasValue && w.GemStatus == GemStatus.Returned).ToListAsync();

                allGemsInLot.ForEach(f => { gemHistorylist.Add(new GemHistory().UpdateGemStatus(f.Id, editedByName, editedById, f.GemStatus, GemStatus.InStock, false, isGemToUpdate.StockNumber, ActionEnum.Edited)); });



                await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Gems] SET GemStatus = {(int)gemStatus}, EditedById={editedById} WHERE GemLotId={isGemToUpdate.GemLotId} AND ShapeId IS NOT NULL UPDATE [dbo].[Gems] SET GemStatus = {(int)GemStatus.GemLot}, EditedById={editedById} WHERE GemLotId={isGemToUpdate.GemLotId} AND ShapeId IS NULL");
            }

            else
            {
                if (!isGemToUpdate.IsGemLot)
                {
                    gemHistorylist.Add(new GemHistory().UpdateGemStatus(gemId, editedByName, editedById, isGemToUpdate.GemStatus, GemStatus.InStock, false, null, ActionEnum.Edited));
                }

                else
                {
                    gemHistorylist.Add(new GemHistory().UpdateGemStatus(gemId, editedByName, editedById, isGemToUpdate.GemStatus, GemStatus.InStock, false, isGemToUpdate.StockNumber, ActionEnum.Edited));

                }
                await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Gems] SET GemStatus = {(int)gemStatus},EditedById={editedById} WHERE Id={gemId}");

                if (isGemToUpdate.IsGemLot && isGemToUpdate.GemStatus == GemStatus.Returned)
                {
                    var gemLotHeader = await gemStoContext.Gems.FirstOrDefaultAsync(f => f.GemLotId == isGemToUpdate.GemLotId && f.IsTreated == null);

                    gemHistorylist.Add(new GemHistory().UpdateGemStatus(gemLotHeader.Id, editedByName, editedById, isGemToUpdate.GemStatus, GemStatus.InStock, true, isGemToUpdate.StockNumber, ActionEnum.Edited));

                    gemLotHeader.InitialCost += isGemToUpdate.InitialCost;
                    gemLotHeader.TotalCost += isGemToUpdate.TotalCost;

                    var paymentStatus = PaymentStatus.Unpaid;

                    if (gemLotHeader.Share == 1.00m)
                    {
                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    else
                    {
                        var payable = (gemLotHeader.InitialCost + gemLotHeader.BrokerFee) * gemLotHeader.Share;

                        paymentStatus = gemLotHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (gemLotHeader.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    gemLotHeader.PaymentStatusToSeller = paymentStatus;

                    var allGemsinlotWithheader = await gemStoContext.Gems.Where(w => w.GemLotId == gemLotHeader.GemLotId).ToListAsync();
                    allGemsinlotWithheader.ForEach(f => f.PaymentStatusToSeller = paymentStatus);

                }
            }
            gemStoContext.GemHistory.AddRange(gemHistorylist);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<NextAndPreviousModel> GetNextAndPreviouseGemIdsAsync(int currentGemId, bool isThirdParty)
        {
            try
            {

                var previous = await gemStoContext.Gems.Where(f => !f.IsDeleted && f.Id < currentGemId && f.IsThirdParty == isThirdParty).OrderByDescending(i => i.Id).FirstOrDefaultAsync();

                var next = await gemStoContext.Gems.Where(f => !f.IsDeleted && f.Id > currentGemId && f.IsThirdParty == isThirdParty).OrderBy(i => i.Id).FirstOrDefaultAsync();

                var nextAndPreviousModel = new NextAndPreviousModel
                {
                    PreviousId = previous?.Id,
                    PreviousIdentity = previous?.Identity,
                    NextId = next?.Id,
                    NextIdentity = next?.Identity
                };
                return nextAndPreviousModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task AddMoreGemsToLotAsync(GemLotCreateList gemLotCreate, int lotId, string lotStockId, string createdByName, string createdById)
        {
            using (var transaction = await gemStoContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var gemHistory = new List<GemHistory>();
                    var entity = await gemStoContext.Gems.Include(i => i.Seller).Where(w => w.StockNumber.StartsWith(lotStockId.Trim())).ToListAsync();

                    var stockNumbers = entity.Select(s => s.StockNumber).ToList();

                    var entityHeader = entity.FirstOrDefault(f => f.Id == lotId);

                    var listOfSubDigits = new List<int>();



                    foreach (var stockNumber in stockNumbers.Where(w => w.Contains('-')).ToList())
                    {
                        var subDigit = int.Parse(stockNumber.Split('-')[1]);
                        listOfSubDigits.Add(subDigit);
                    }

                    listOfSubDigits.Sort();

                    var nextSubStockId = listOfSubDigits.Last() + 1;


                    var gemToAddToLot = new Gem
                    {
                        StockNumber = lotStockId + "-" + nextSubStockId++.ToString(),
                        InitialCost = gemLotCreate.InitialCost,
                        InitialWeight = gemLotCreate.InitialWeight,
                        IsTreated = gemLotCreate.IsTreated,
                        ShapeId = gemLotCreate.Shape.Id,
                        VarietyId = gemLotCreate.Variety.Id,
                        TotalCost = gemLotCreate.InitialCost,
                        GemStatus = GemStatus.InStock,
                        NumberOfPieces = 1,
                        SellerName = entityHeader.SellerName,
                        SellerId = entityHeader.SellerId,
                        GemLotId = entityHeader.GemLotId,
                        Length = gemLotCreate.Length ?? 0,
                        Width = gemLotCreate.Width ?? 0,
                        Depth = gemLotCreate.Depth ?? 0,
                        IsGemLot = true,
                        Share = entityHeader.Share,
                        PaymentStatusToSeller = entityHeader.PaymentStatusToSeller
                    };

                    gemHistory.Add(new GemHistory().CreateNewGemAddedToLot(gemId: entityHeader.Id, description: $"Added new gem to lot with stock no.: {gemToAddToLot.StockNumber}", createdByName, createdById, gemStatus: entityHeader.GemStatus));

                    entityHeader.NumberOfPieces += 1;
                    entityHeader.InitialCost += gemLotCreate.InitialCost;
                    entityHeader.InitialWeight += gemLotCreate.InitialWeight;
                    entityHeader.TotalCost += gemLotCreate.InitialCost;

                    var costOfAllGemsInLot = entity.Where(w => w.IsTreated != null && (w.GemStatus != GemStatus.Lost || w.GemStatus != GemStatus.Returned)).ToList().Sum(x => x.InitialCost) + gemLotCreate.InitialCost;


                    var paymentStatus = PaymentStatus.Unpaid;

                    if (entityHeader.Share == 1.00m)
                    {
                        paymentStatus = entityHeader.TotalAmountPaidToSeller == 0 ? PaymentStatus.Unpaid :
                        ((costOfAllGemsInLot + entityHeader.BrokerFee) > entityHeader.TotalAmountPaidToSeller ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    else
                    {
                        var payable = (entityHeader.InitialCost + entityHeader.BrokerFee) * entityHeader.Share;

                        paymentStatus = entityHeader.TotalAmountPaidToSeller == 0 ?
                                                PaymentStatus.Unpaid : (entityHeader.TotalAmountPaidToSeller < payable ? PaymentStatus.Partial : PaymentStatus.Paid);
                    }
                    entityHeader.PaymentStatusToSeller = paymentStatus;

                    var allGemsinlotWithheader = await gemStoContext.Gems.Where(w => w.GemLotId == entityHeader.GemLotId).ToListAsync();
                    allGemsinlotWithheader.ForEach(f => f.PaymentStatusToSeller = paymentStatus);


                    gemStoContext.Entry(entityHeader).State = EntityState.Modified;
                    gemStoContext.Gems.Add(gemToAddToLot);

                    await gemStoContext.SaveChangesAsync();

                    gemHistory.Add(new GemHistory().CreateNewGemAddedToLot(gemId: gemToAddToLot.Id, description: $"Purchased gem from {entityHeader.SellerName ?? entityHeader.Seller?.Name}", createdByName, createdById, gemStatus: gemToAddToLot.GemStatus));

                    gemStoContext.GemHistory.AddRange(gemHistory);
                    await gemStoContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }

                transaction.Commit();
            }
        }

        public async Task<PaginationModel<GemModel>> GlobalSearchAsync(GlobalSearchFilterModel globalSearchFilterModel, int skip, int take, bool isThirdParty)
        {
            try
            {
                var query = gemStoContext.Gems.Include(i => i.RecutShape).Where(w => !w.IsDeleted && w.GemStatus != GemStatus.Returned && w.IsThirdParty == isThirdParty).AsQueryable();

                if (globalSearchFilterModel.CertificateProviderId != null && globalSearchFilterModel.CertificateProviderId.Count() != 0)
                {
                    query = query.Where(w => w.Certificates.Any(a => globalSearchFilterModel.CertificateProviderId.Contains(a.CertificateProviderId)));
                }

                if (globalSearchFilterModel.ColorId != null && globalSearchFilterModel.ColorId.Count() != 0)
                {
                    query = query.Where(w => w.Certificates.Any(a => globalSearchFilterModel.ColorId.Contains(a.ColourId)));
                }

                if (globalSearchFilterModel.MaxCost.HasValue)
                {
                    query = query.Where(w => w.TotalCost <= globalSearchFilterModel.MaxCost.Value);
                }

                if (globalSearchFilterModel.MaxWeight.HasValue)
                {
                    query = query.Where(w => (w.RecutWeight != 0.00m && w.RecutWeight <= globalSearchFilterModel.MaxWeight.Value) || (w.RecutWeight == 0.00m && w.InitialWeight <= globalSearchFilterModel.MaxWeight.Value));
                }

                if (globalSearchFilterModel.MinCost.HasValue)
                {
                    query = query.Where(w => w.TotalCost >= globalSearchFilterModel.MinCost.Value);
                }

                if (globalSearchFilterModel.MinWeight.HasValue)
                {
                    query = query.Where(w => (w.RecutWeight != 0.00m && w.RecutWeight >= globalSearchFilterModel.MinWeight.Value) || (w.RecutWeight == 0.00m && w.InitialWeight >= globalSearchFilterModel.MinWeight.Value));
                }

                if (globalSearchFilterModel.PaymentStatus != null && globalSearchFilterModel.PaymentStatus.Count() != 0)
                {
                    query = query.Where(w => globalSearchFilterModel.PaymentStatus.Contains(w.PaymentStatusToSeller));
                }

                if (globalSearchFilterModel.GemStatus != null && globalSearchFilterModel.GemStatus.Count() != 0)
                {
                    query = query.Where(w => globalSearchFilterModel.GemStatus.Contains(w.GemStatus));
                }

                if (!string.IsNullOrEmpty(globalSearchFilterModel.Seller))
                {
                    query = query.Where(w => w.SellerName == globalSearchFilterModel.Seller || w.Seller.Name == globalSearchFilterModel.Seller);
                }

                if (globalSearchFilterModel.ShapeId != null && globalSearchFilterModel.ShapeId.Count() != 0)
                {
                    query = query.Where(w => (globalSearchFilterModel.ShapeId.Contains(w.RecutShapeId) && w.RecutShapeId != null) || (globalSearchFilterModel.ShapeId.Contains(w.ShapeId) && w.RecutShapeId == null));
                }

                if (globalSearchFilterModel.VarietyId != null && globalSearchFilterModel.VarietyId.Count() != 0)
                {
                    query = query.Where(w => globalSearchFilterModel.VarietyId.Contains(w.VarietyId));
                }

                if (!string.IsNullOrEmpty(globalSearchFilterModel.CertificateNumber))
                {
                    query = query.Where(w => w.Certificates.Any(a => EF.Functions.Like(a.Number, "%" + globalSearchFilterModel.CertificateNumber + "%")));
                }

                // *************  dimension rounding start ***************************************************************
                if (globalSearchFilterModel.Length.HasValue)
                {
                    if (globalSearchFilterModel.Length.Value.ToString().Length < 4)
                    {
                        query = query.Where(w => EF.Functions.Like(w.Length.ToString(), globalSearchFilterModel.Length.Value.ToString() + "%"));
                    }
                    else
                    {
                        query = query.Where(w => w.Length == globalSearchFilterModel.Length.Value);
                    }
                }

                if (globalSearchFilterModel.Width.HasValue)
                {
                    if (globalSearchFilterModel.Width.Value.ToString().Length < 4)
                    {
                        query = query.Where(w => EF.Functions.Like(w.Width.ToString(), globalSearchFilterModel.Width.Value.ToString() + "%"));
                    }
                    else
                    {
                        query = query.Where(w => w.Width == globalSearchFilterModel.Width.Value);
                    }
                }

                if (globalSearchFilterModel.Depth.HasValue)
                {
                    if (globalSearchFilterModel.Depth.Value.ToString().Length < 4)
                    {
                        query = query.Where(w => EF.Functions.Like(w.Depth.ToString(), globalSearchFilterModel.Depth.Value.ToString() + "%"));
                    }

                    else
                    {
                        query = query.Where(w => w.Depth == globalSearchFilterModel.Depth.Value);
                    }
                }
                // *************  dimension rounding end ***************************************************************


                if (globalSearchFilterModel.IsNatural.HasValue && !globalSearchFilterModel.IsHeated.HasValue)
                {
                    query = query.Where(w => w.IsTreated == false);
                }

                if (globalSearchFilterModel.IsHeated.HasValue && !globalSearchFilterModel.IsNatural.HasValue)
                {
                    query = query.Where(w => w.IsTreated == true);
                }

                if (globalSearchFilterModel.PurchasedFrom.HasValue)
                {
                    query = query.Where(w => w.PurchasedDate >= globalSearchFilterModel.PurchasedFrom.Value);
                }

                if (globalSearchFilterModel.PurchasedTo.HasValue)
                {
                    query = query.Where(w => w.PurchasedDate <= globalSearchFilterModel.PurchasedTo.Value);
                }


                var totalNumberOfRecord = await query.AsNoTracking().CountAsync();

                query = query.OrderByDescending(x => x.Id).Skip(skip).Take(take);

                var result = query.Select(x => new GemModel
                {
                    Id = x.Id,
                    StockNumber = x.StockNumber,
                    Variety = new VarietyModel
                    {
                        Value = x.Variety.Value
                    },
                    InitialWeight = x.InitialWeight,
                    RecutWeight = x.RecutWeight,
                    IsTreated = x.IsTreated,
                    InitialCost = x.InitialCost,
                    TotalCost = x.TotalCost,
                    Shape = new ShapeModel
                    {
                        Value = x.RecutShapeId != null ? x.RecutShape.Value : x.Shape.Value,
                        ImagePath = x.RecutShapeId != null ? x.RecutShape.ImagePath : x.Shape.ImagePath
                    },
                    SellerName = x.SellerId != null ? x.Seller.Name : x.SellerName,
                    PaymentStatusToSeller = x.PaymentStatusToSeller,
                    TotalAmountPaidToSeller = x.TotalAmountPaidToSeller,
                    PurchasedDate = x.PurchasedDate.Value,
                    GemStatus = x.GemStatus, //will be automatically updated when inserting new gem or editing payment amount
                    NumberOfPieces = x.NumberOfPieces,
                    IsGemLot = x.IsGemLot,
                    Identity = x.Identity,
                    Share = x.Share
                }).AsQueryable();

                var resultData = await result.AsNoTracking().ToListAsync();

                var resultSet = new PaginationModel<GemModel>()
                {
                    TotalRecords = totalNumberOfRecord,
                    Details = resultData
                };

                return resultSet;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<GemModel> GetByintIdAsync(int gemIntId)
        {
            try
            {
                var query = gemStoContext.Gems
                    .Include(i => i.Shape).Include(i => i.RecutShape).Include(i => i.Certificates).Include(i => i.Variety).Include(i => i.Seller).Include(i => i.Transactions)
               .Where(w => w.Id == gemIntId && !w.IsDeleted).AsQueryable();

                var entity = await query.FirstOrDefaultAsync();

                var model = mapper.Map<GemModel>(entity);
                if (model.SellerName != null)
                    model.Seller = null;

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GemModel>> GetByintIdSaleCalculatorAsync(int gemIntId)
        {
            try
            {
                var query = gemStoContext.Gems
                    .Include(i => i.Shape).Include(i => i.RecutShape).Include(i => i.Certificates).Include(i => i.Variety).Include(i => i.Seller).Include(i => i.Transactions)
               .Where(w => w.Id == gemIntId && !w.IsDeleted).AsQueryable();

                var entity = await query.ToListAsync();

                var model = mapper.Map<List<GemModel>>(entity);

                if (model.FirstOrDefault().IsTreated == null)
                {
                    var lotQuery = await gemStoContext.Gems
                                        .Include(i => i.Shape).Include(i => i.RecutShape).Include(i => i.Certificates).Include(i => i.Variety).Include(i => i.Seller).Include(i => i.Transactions)
                                   .Where(w => w.GemLotId == entity.FirstOrDefault().GemLotId && w.IsTreated != null && !w.IsDeleted).ToListAsync();
                    model.AddRange(mapper.Map<List<GemModel>>(lotQuery));


                }
                foreach (var item in model)
                {
                    if (item.SellerName != null)
                        item.Seller = null;
                }

                return model.OrderBy(o => o.StockNumber).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaginationModel<GemModel>> GetUnPaidAmountBySellerAsync(string sellerName, bool isPrintable)
        {
            var query = gemStoContext.Gems.Where(w => !w.IsDeleted && w.GemStatus != GemStatus.Returned && w.IsThirdParty == false &&
            (w.PaymentStatusToSeller == PaymentStatus.Partial || w.PaymentStatusToSeller == PaymentStatus.Unpaid) &&
            (w.Seller.Name == sellerName || w.SellerName == sellerName)).OrderByDescending(x => x.Id);


            var result = query.Select(x => new GemModel
            {
                Id = x.Id,
                StockNumber = x.StockNumber,
                Variety = new VarietyModel
                {
                    Value = x.Variety.Value
                },
                InitialWeight = x.InitialWeight,
                RecutWeight = x.RecutWeight,
                IsTreated = x.IsTreated,
                InitialCost = x.InitialCost,
                TotalCost = x.TotalCost,
                Shape = new ShapeModel
                {
                    Value = x.RecutShapeId != null ? x.RecutShape.Value : x.Shape.Value,
                    ImagePath = x.RecutShapeId != null ? x.RecutShape.ImagePath : x.Shape.ImagePath
                },
                SellerName = x.SellerId != null ? x.Seller.Name : x.SellerName,
                PaymentStatusToSeller = x.PaymentStatusToSeller,
                TotalAmountPaidToSeller = x.TotalAmountPaidToSeller,
                PurchasedDate = x.PurchasedDate.Value,
                GemStatus = x.GemStatus,
                NumberOfPieces = x.NumberOfPieces,
                IsGemLot = x.IsGemLot,
                Identity = x.Identity,
                BrokerFee = x.BrokerFee,
                Share = x.Share
            }).AsQueryable();

            var totalUnpaid = 0.00m;
            var totalPaid = 0.00m;
            var totalBrokerFee = 0.00m;


            foreach (var gem in result)
            {
                if (!gem.IsGemLot || gem.GemStatus == GemStatus.GemLot)
                {

                    totalUnpaid += !isPrintable ? ((gem.InitialCost + gem.BrokerFee) * gem.Share) - gem.TotalAmountPaidToSeller : (gem.InitialCost + gem.BrokerFee - gem.TotalAmountPaidToSeller);
                    totalPaid += gem.TotalAmountPaidToSeller;
                    totalBrokerFee += gem.BrokerFee;
                }
            }

            var resultDate = await result.AsNoTracking().ToListAsync();

            var totalCount = resultDate.Count(c => c.GemStatus != GemStatus.GemLot);

            var resultSet = new PaginationModel<GemModel>
            {
                Details = resultDate,
                TotalRecords = totalCount,
                ExtensionData = new { totalUnpaid, totalPaid, totalBrokerFee }
            };
            return resultSet;
        }

        public async Task<List<GemHistoryModel>> GetGemHistoryAsync(int gemId)
        {
            var query = gemStoContext.GemHistory.Where(w => w.GemId == gemId).OrderByDescending(o => o.CreatedOn);

            var resultSet = await query.Select(s => new GemHistoryModel
            {
                Action = Enum.GetName(typeof(ActionEnum), s.ActionEnum),
                CreatedByName = s.CreatedByName,
                Date = s.CreatedOn.LocalDateTime.ToString("dd MMM yyyy"),
                Description = s.Description,
                GemStatusString = s.GemStatus.DescriptionAttr().ToUpper()
            }).AsNoTracking().ToListAsync();

            return resultSet;
        }

        public async Task<List<KeyValuePair<int, string>>> GetPaymentIncompleteGemsAsync(string sellerName)
        {
            var query = gemStoContext.Gems.Where(w => !w.IsDeleted && w.GemStatus != GemStatus.Returned &&
                (w.PaymentStatusToSeller == PaymentStatus.Partial || w.PaymentStatusToSeller == PaymentStatus.Unpaid) &&
                (w.Seller.Name == sellerName || w.SellerName == sellerName) && (!w.IsGemLot || (w.IsGemLot && w.IsTreated == null))).OrderBy(x => x.Id);


            var result = await query.Select(x => new KeyValuePair<int, string>(x.Id, x.StockNumber)).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<GemModel>> GetPaymentIncompleteGemsForBulkPaymentAsync(string sellerName)
        {
            var query = gemStoContext.Gems.Include(i => i.Shape).Include(i => i.Variety).Where(w => w.IsThirdParty == false && !w.IsDeleted && w.GemStatus != GemStatus.Returned &&
            (w.PaymentStatusToSeller == PaymentStatus.Partial || w.PaymentStatusToSeller == PaymentStatus.Unpaid) &&
            (w.Seller.Name == sellerName || w.SellerName == sellerName) && (!w.IsGemLot || (w.IsGemLot && w.IsTreated == null))).OrderBy(x => x.Id);


            var entityList = await query.ToListAsync();

            var modelList = mapper.Map<IEnumerable<GemModel>>(entityList);

            return modelList;
        }

    }
}

using AutoMapper;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain;
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
    public class ThirdPartyGemService : IThirdPartyGemService
    {
        private readonly GemStoContext _gemStoContext;
        private readonly IMapper _mapper;

        public ThirdPartyGemService(GemStoContext gemStoContext, IMapper mapper)
        {
            _gemStoContext = gemStoContext;
            _mapper = mapper;
        }

        public async Task CreateNewThirdPartyGemAsync(SignleGemCreateModel gemModel)
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
                    Share = gemModel.Share,
                    IsThirdParty = true
                };

                var savedEntity = _gemStoContext.Gems.Add(entity);

                var sellerName = gemModel.SellerName;
                if (sellerName is null)
                {
                    sellerName = await _gemStoContext.Sellers.Where(w => w.Id == (int)gemModel.Seller.Id).Select(s => s.Name).FirstOrDefaultAsync();
                }

                await _gemStoContext.SaveChangesAsync();

                var gemPurchasedHistory = new GemHistory().CreateSingleGemTPReceived(savedEntity.Entity.Id, sellerName, gemModel.CreatedByName, gemModel.CreatedById, entity.GemStatus);

                _gemStoContext.GemHistory.Add(gemPurchasedHistory);
                await _gemStoContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.StartsWith("Cannot insert duplicate key row in object"))
                    throw new Exception($"Stock number {gemModel.StockNumber} has been taken, save again with the updated third party stock number");
                else
                    throw new Exception(ex.Message);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveGemLotAsync(GemLotListCreateModel gemLotListCreateModel)
        {
            using (var transaction = await _gemStoContext.Database.BeginTransactionAsync())
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
                        Share = gemLotListCreateModel.Share,
                        IsThirdParty = true
                    };

                    list.Add(lotSummary);

                    var counter = 1;

                    var gemLot = new GemLot
                    {
                        TotalAmountPaidToSeller = gemLotListCreateModel.GemLotCost,
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
                            PaymentStatusToSeller = lotSummary.PaymentStatusToSeller,
                            IsThirdParty = true
                        });
                    }

                    list.Reverse();

                    await _gemStoContext.GemLots.AddAsync(gemLot);
                    await _gemStoContext.SaveChangesAsync();

                    var sellerName = gemLotListCreateModel.SellerName;
                    if (sellerName is null)
                    {
                        sellerName = await _gemStoContext.Sellers.Where(w => w.Id == (int)gemLotListCreateModel.Seller.Id).Select(s => s.Name).FirstOrDefaultAsync();
                    }
                    foreach (var gem in gemLot.Gems)
                    {
                        gemHistory.Add(new GemHistory().CreateSingleGemPurchased(gem.Id, sellerName, gemLotListCreateModel.CreatedByName, gemLotListCreateModel.CreatedById, gem.GemStatus));
                    }

                    _gemStoContext.GemHistory.AddRange(gemHistory);
                    await _gemStoContext.SaveChangesAsync();

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
    }
}

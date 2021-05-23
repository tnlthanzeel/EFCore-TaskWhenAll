using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Models.Sale;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class SalePaymentsService : ISalePaymentsService
    {
        private readonly GemStoContext gemstoContext;
        private readonly IMapper mapper;

        public SalePaymentsService(GemStoContext gemstoContext, IMapper mapper)
        {
            this.gemstoContext = gemstoContext;
            this.mapper = mapper;
        }

        public async Task<PaginationModel<SellerPaymentHistoryModel>> GetBuyerPaymentHistory(int skip = 0, int take = 75)
        {
            var query = gemstoContext.SalePayments.Where(w => !w.IsDeleted && !w.GemSales.IsDeleted && w.GemSales.TotalAmountPaid != -1.00m && w.IsPaymentApproved == true);

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(o => o.CreatedOn).Skip(skip).Take(take);

            var result = query.Select(s => new SellerPaymentHistoryModel
            {
                SellerName = s.GemSales.BuyerId.HasValue ? s.GemSales.Buyer.Name : s.GemSales.BuyerName,
                StockNumber = s.GemSales.SaleNumber,
                PaidAmount = (s.AmmountPaid * s.GemSales.SellingRate),
                PaidOn = s.CreatedOn,
                IsSinglePurchase = s.GemSales.IsSingleSale,
                GemId = s.GemSalesId,
                GemIdentity = (Guid)s.GemSales.SaleId,
                PaymentHistoryType = PaymentHistoryType.Buyer,
                IsThirdParty = s.GemSales.IsThirdParty,
                Id = s.GemSalesId,
                GemStockNumber = s.GemSales.GemId.HasValue ? s.GemSales.Gem.StockNumber : s.GemSales.ThirdPartyOwner,
            });

            var resultData = await result.AsNoTracking().ToListAsync();

            var resultSet = new PaginationModel<SellerPaymentHistoryModel>()
            {
                Details = resultData,
                TotalRecords = totalCount
            };

            return resultSet;
        }
    }
}

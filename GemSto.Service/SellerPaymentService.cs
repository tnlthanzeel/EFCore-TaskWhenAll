using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class SellerPaymentService : ISellerPaymentService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public SellerPaymentService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }

        public async Task<PaginationModel<SellerPaymentHistoryModel>> GetSellerPaymentHistoryAsync(int skipe, int take)
        {
            var query = gemStoContext.Transactions.Include(i => i.Gem).ThenInclude(i => i.Seller)
                                     .Where(w => !w.IsDeleted && w.Remark != Remark.Deleted && w.Gem.GemStatus != GemStatus.Returned &&
                                            w.TransactionType == TransactionType.SellerPayment &&
                                            w.PaidAmount != 0.00m
                                           );


            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(o => o.PaidOn).Skip(skipe).Take(take);

            var result = query.Select(s => new SellerPaymentHistoryModel
            {
                SellerName = s.Gem.SellerId.HasValue ? s.Gem.Seller.Name : s.Gem.SellerName,
                StockNumber = s.Gem.StockNumber,
                PaidAmount = s.PaidAmount,
                PaidOn = (DateTimeOffset)s.PaidOn,
                IsSinglePurchase = s.GemStatus == GemStatus.GemLot ? false : true,
                GemId = s.GemId,
                GemIdentity = s.Gem.Identity,
                PaymentHistoryType = PaymentHistoryType.Seller,
                Id = s.Id,
                Description = s.Description
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

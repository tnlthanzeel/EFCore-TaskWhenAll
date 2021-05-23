using GemSto.Common;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Stores.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Stores
{
    public class PaymentStore : IPaymentStore
    {
        private readonly ISellerPaymentService _sellerPaymentService;
        private readonly ISalePaymentsService _salePaymentsService;
        private readonly IMiscPaymentService _miscPaymentService;

        public PaymentStore(ISellerPaymentService sellerPaymentService, ISalePaymentsService salePaymentsService, IMiscPaymentService miscPaymentService)
        {
            _sellerPaymentService = sellerPaymentService;
            _salePaymentsService = salePaymentsService;
            _miscPaymentService = miscPaymentService;
        }
        public async Task<PaginationModel<SellerPaymentHistoryModel>> GetPaymentHistory(int skip = 0, int take = 75)
        {
            try
            {
                var taskList = new List<Task<PaginationModel<SellerPaymentHistoryModel>>>();

                var resultSet = new PaginationModel<SellerPaymentHistoryModel>();

                var paymentHistories = new List<SellerPaymentHistoryModel>();

                var sellerHisotryTask = _sellerPaymentService.GetSellerPaymentHistoryAsync(skip, take);

                var buyerHistoryTask = _salePaymentsService.GetBuyerPaymentHistory(skip, take);

                var miscPaymentHistoryTask = _miscPaymentService.GetMiscPaymentHistoryAsync(skip, take);

                taskList.AddRange(new[] { sellerHisotryTask, buyerHistoryTask, miscPaymentHistoryTask });

                await Task.WhenAll(taskList);

                var sellerHisotry = await sellerHisotryTask;
                var buyerHistory = await buyerHistoryTask;
                var miscPaymentHistory = await miscPaymentHistoryTask;


                paymentHistories.AddRange(sellerHisotry.Details);
                paymentHistories.AddRange(buyerHistory.Details);
                paymentHistories.AddRange(miscPaymentHistory.Details);


                resultSet.Details = paymentHistories.OrderByDescending(o => o.PaidOn).ToList();
                resultSet.TotalRecords = sellerHisotry.TotalRecords + buyerHistory.TotalRecords + miscPaymentHistory.TotalRecords;

                return resultSet;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}

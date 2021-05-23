using GemSto.Common;
using GemSto.Service.Models;
using GemSto.Service.Models.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ISalePaymentsService
    {
        Task<PaginationModel<SellerPaymentHistoryModel>> GetBuyerPaymentHistory(int skip = 0, int take = 75);
    }
}

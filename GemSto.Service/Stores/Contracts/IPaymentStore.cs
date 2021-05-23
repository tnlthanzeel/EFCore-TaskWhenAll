using GemSto.Common;
using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Stores.Contracts
{
    public interface IPaymentStore
    {
        Task<PaginationModel<SellerPaymentHistoryModel>> GetPaymentHistory(int skip = 0, int take = 75);
    }
}

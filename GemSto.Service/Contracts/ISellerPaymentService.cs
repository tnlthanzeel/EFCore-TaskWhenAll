using GemSto.Common;
using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ISellerPaymentService
    {
        Task<PaginationModel<SellerPaymentHistoryModel>> GetSellerPaymentHistoryAsync(int skip, int take);
    }
}

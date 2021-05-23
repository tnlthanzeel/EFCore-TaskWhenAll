using GemSto.Common;
using GemSto.Service.Models;
using GemSto.Service.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IMiscPaymentService
    {
        Task<PaginationModel<SellerPaymentHistoryModel>> GetMiscPaymentHistoryAsync(int skip = 0, int take = 75);

    }
}

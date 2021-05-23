using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IBuyerLookUpService
    {
        Task CreateBuyerAsync(BuyerCreateModel buyerCreateModel);
        Task<IEnumerable<BuyerModel>> GetAllBuyersAsync();
        Task<BuyerModel> GetBuyerByIdAsync(Guid id);
        Task DeleteBuyerAsync(Guid id);
        Task UpdateBuyerAsync(BuyerUpdateModel buyerUpdateModel);
        Task<List<string>> GetAllBuyerNamesAsync();
    }
}

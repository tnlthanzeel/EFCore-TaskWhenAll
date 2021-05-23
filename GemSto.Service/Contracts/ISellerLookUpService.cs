using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ISellerLookUpService
    {
        Task<IEnumerable<SellerModel>> GetSellerKeyValuePairAsync();
        Task<bool> AddNewSeller(SellerModel sellerModel);
        Task DeleteSeller(int id);
        Task<SellerModel> GetSellerByIdAsync(int id);
        Task UpdateSellerAsync(SellerModel sellerModel);
        Task<List<string>>GetAllSellerNamesAsync();
    }
}

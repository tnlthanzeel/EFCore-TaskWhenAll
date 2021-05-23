using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IOriginLookUpService
    {
        Task<bool> AddNewOriginAsync(OriginModel originModel);
        Task DeleteOrigin(int id);
        Task UpdateOriginAsync(OriginModel originModel);
        Task<List<OriginModel>> GetAllOriginAsync();
        Task<OriginModel> GetOriginByIdAsync(int id);

    }
}

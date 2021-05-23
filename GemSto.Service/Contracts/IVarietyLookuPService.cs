using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IVarietyLookUpService
    {
        Task<IEnumerable<VarietyModel>> GetVarietyKeyValuePairAsync();
        Task<bool> AddNewVarietyAsync(VarietyCreateModel varietyCreateModel);
        Task DeleteVarietyAsync(int id);
        Task<VarietyModel> GetVarietyByIdAsync(int id);
        Task UpdateVarietyAsync(VarietyModel varietyModel);
    }
}

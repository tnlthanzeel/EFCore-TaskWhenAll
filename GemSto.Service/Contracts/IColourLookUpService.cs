using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IColourLookUpService
    {
        Task<bool> AddNewColourAsync(ColourCreateModel colourCreateModel);
        Task<IEnumerable<ColourModel>> GetAllColours();
        Task DeleteColourAsync(int id);
        Task UpdateColourAync(ColourModel colourModel);
        Task<ColourModel> GetColourByIdAync(int id);
    }
}

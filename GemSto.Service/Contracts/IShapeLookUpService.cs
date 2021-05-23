using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IShapeLookUpService
    {
        Task<IEnumerable<ShapeModel>> GetShapeKeyValuePairAsync();
        Task<bool> AddNewShapeAsync(ShapeCreateModel shapeCreateModel);
        Task DeleteShapeAsync(int id);
        Task<ShapeModel> GetShapeByIdAsync(int id);
        Task<bool> UpdateShapeAsync(ShapeCreateModel shapeUpdateModel);
    }
}

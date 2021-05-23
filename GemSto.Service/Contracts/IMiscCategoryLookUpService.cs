using GemSto.Service.Models.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IMiscCategoryLookUpService
    {
        Task<List<MiscCategoryModel>> GetAllMiscCategories();
        Task<bool> AddNewMiscCategoryAsync(MiscCategoryCreateModel miscCategoryCreateModel);
        Task<bool> MiscCategoryDeleteAsync(int id);
        Task<MiscCategoryModel> MiscCategoryGetAsync(int id);
        Task<bool> MiscCategoryUpdateAsync(MiscCategoryModel miscCategoryModel);

        Task<List<MiscCategoryModel>> GetAllMiscCategoriesHavingSubCategoriesAsync();
        Task<bool> AddNewMiscSubCategoryAsync(MiscSubCategoryCreateModel miscSubCategoryCreateModel);
        Task<List<MiscSubCategoryModel>> GetAllMiscSubCategoriesAsync(int miscCatId);
        Task DeleteMiscSubCategoryAsync(int miscSubCatId);
        Task<MiscSubCategoryModel> MiscSubCategoryGetAsync(int id);
        Task<bool> MiscSubCategoryUpdateAsync(MiscSubCategoryUdateModel miscSubCategoryUdateModel);
    }
}

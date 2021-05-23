using AutoMapper;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class MiscCategoryLookUpService : IMiscCategoryLookUpService
    {
        private readonly GemStoContext _gemStoContext;
        private readonly IMapper _mapper;

        public MiscCategoryLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            _gemStoContext = gemStoContext;
            _mapper = mapper;
        }
        public async Task<List<MiscCategoryModel>> GetAllMiscCategories()
        {
            var catList = await _gemStoContext.MiscCategory.Where(w => w.IsDeleted == false)
                .OrderBy(o => o.Value)
                .Select(s => new MiscCategoryModel
                {
                    Id = s.Id,
                    HasSubCategory = s.HasSubCategory,
                    Value = s.Value
                }).ToListAsync();

            return catList;
        }

        public async Task<bool> AddNewMiscCategoryAsync(MiscCategoryCreateModel miscCategoryCreateModel)
        {
            var isAny = await _gemStoContext.MiscCategory.AnyAsync(x => x.Value == miscCategoryCreateModel.Value && x.HasSubCategory == miscCategoryCreateModel.HasSubCategory && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }
            else
            {
                var entity = _mapper.Map<MiscCategory>(miscCategoryCreateModel);
                _gemStoContext.MiscCategory.Add(entity);
                await _gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> AddNewMiscSubCategoryAsync(MiscSubCategoryCreateModel miscSubCategoryCreateModel)
        {
            var isAny = await _gemStoContext.MiscSubCategories.AnyAsync(x => x.Value == miscSubCategoryCreateModel.Value && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }
            else
            {
                var entity = _mapper.Map<MiscSubCategory>(miscSubCategoryCreateModel);
                _gemStoContext.MiscSubCategories.Add(entity);
                await _gemStoContext.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> MiscCategoryDeleteAsync(int id)
        {
            var entity = await _gemStoContext.MiscCategory.Include(i => i.MiscSubCategories).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            if (entity.MiscSubCategories.Any(a => a.IsDeleted == false))
            {
                return false;
            }
            else
            {
                entity.IsDeleted = true;
                await _gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<MiscCategoryModel> MiscCategoryGetAsync(int id)
        {
            var entity = await _gemStoContext.MiscCategory.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            var model = _mapper.Map<MiscCategoryModel>(entity);
            return model;
        }


        public async Task<bool> MiscCategoryUpdateAsync(MiscCategoryModel miscCategoryModel)
        {
            var isAny = await _gemStoContext.MiscCategory.AnyAsync(x => x.Value == miscCategoryModel.Value && x.HasSubCategory == miscCategoryModel.HasSubCategory && !x.IsDeleted && x.Id != miscCategoryModel.Id);
            if (isAny)
            {
                return false;
            }
            var entity = await _gemStoContext.MiscCategory.FirstOrDefaultAsync(f => f.Id == miscCategoryModel.Id);
            entity = _mapper.Map(miscCategoryModel, entity);
            _gemStoContext.MiscCategory.Update(entity);
            await _gemStoContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<MiscCategoryModel>> GetAllMiscCategoriesHavingSubCategoriesAsync()
        {
            var catList = await _gemStoContext.MiscCategory.Where(w => w.IsDeleted == false && w.HasSubCategory == true)
                .OrderBy(o => o.Value)
                .Select(s => new MiscCategoryModel
                {
                    Id = s.Id,
                    HasSubCategory = s.HasSubCategory,
                    Value = s.Value
                }).ToListAsync();

            return catList;
        }

        public async Task<List<MiscSubCategoryModel>> GetAllMiscSubCategoriesAsync(int miscCatId)
        {
            var subCatList = await _gemStoContext.MiscSubCategories.Where(w => w.IsDeleted == false && w.MiscCategoryId == miscCatId)
                .OrderBy(o => o.Value)
                .Select(s => new MiscSubCategoryModel
                {
                    Id = s.Id,
                    MiscCategoryId = s.MiscCategoryId,
                    Value = s.Value
                }).ToListAsync();

            return subCatList;
        }

        public async Task DeleteMiscSubCategoryAsync(int miscSubCatId)
        {
            var subCatList = await _gemStoContext.MiscSubCategories.FirstOrDefaultAsync(w => w.IsDeleted == false && w.Id == miscSubCatId);

            subCatList.IsDeleted = true;
            await _gemStoContext.SaveChangesAsync();
        }


        public async Task<MiscSubCategoryModel> MiscSubCategoryGetAsync(int id)
        {
            var entity = await _gemStoContext.MiscSubCategories.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            var model = _mapper.Map<MiscSubCategoryModel>(entity);
            return model;
        }

        public async Task<bool> MiscSubCategoryUpdateAsync(MiscSubCategoryUdateModel miscSubCategoryUdateModel)
        {
            //var isAny = await _gemStoContext.MiscSubCategories.AnyAsync(x => x.Value == miscSubCategoryUdateModel.Value && !x.IsDeleted);
            //if (isAny)
            //{
            //    return false;
            //}
            var entity = await _gemStoContext.MiscSubCategories.FirstOrDefaultAsync(f => f.Id == miscSubCategoryUdateModel.Id);
            entity = _mapper.Map(miscSubCategoryUdateModel, entity);
            _gemStoContext.MiscSubCategories.Update(entity);
            await _gemStoContext.SaveChangesAsync();
            return true;
        }
    }
}

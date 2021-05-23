using AutoMapper;
using GemSto.Data;
using GemSto.Domain.LookUp;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class VarietyLookUpService : IVarietyLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public VarietyLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<VarietyModel>> GetVarietyKeyValuePairAsync()
        {
            var query = gemStoContext.Varieties.Where(w => !w.IsDeleted).OrderBy(o => o.Value).AsQueryable();
            var entity = await query.AsNoTracking().ToListAsync();
            var model = mapper.Map<List<VarietyModel>>(entity);
            return model;
        }

        public async Task<bool> AddNewVarietyAsync(VarietyCreateModel varietyCreateModel)
        {
            var isAny = await gemStoContext.Varieties.AnyAsync(x => x.Value == varietyCreateModel.Value && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }

            else
            {
                var entity = mapper.Map<Variety>(varietyCreateModel);
                await gemStoContext.Varieties.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task DeleteVarietyAsync(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Varieties] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task<VarietyModel> GetVarietyByIdAsync(int id)
        {
            var entity = await gemStoContext.Varieties.FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<VarietyModel>(entity);
            return model;
        }

        public async Task UpdateVarietyAsync(VarietyModel varietyModel)
        {
            var entity = mapper.Map<Variety>(varietyModel);
            gemStoContext.Update(entity);
            await gemStoContext.SaveChangesAsync();
        }
    }
}

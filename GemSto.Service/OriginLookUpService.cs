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
    public class OriginLookUpService : IOriginLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public OriginLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<bool> AddNewOriginAsync(OriginModel originModel)
        {
            var isAny = await gemStoContext.Origins.AnyAsync(a => !a.IsDeleted && a.Value == originModel.Value);
            if (isAny)
            {
                return false;
            }

            var entity = mapper.Map<Origin>(originModel);
            await gemStoContext.Origins.AddAsync(entity);
            await gemStoContext.SaveChangesAsync();
            return true;
        }

        public async Task DeleteOrigin(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Origins] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task<List<OriginModel>> GetAllOriginAsync()
        {
            var query = gemStoContext.Origins.Where(w => !w.IsDeleted).OrderBy(o => o.Value).OrderBy(o => o.Value).AsQueryable();
            var entity = await query.AsNoTracking().ToListAsync();
            var modelList = mapper.Map<List<OriginModel>>(entity);
            return modelList;
        }

        public async Task<OriginModel> GetOriginByIdAsync(int id)
        {
            var entity = await gemStoContext.Origins.FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<OriginModel>(entity);
            return model;
        }

        public async Task UpdateOriginAsync(OriginModel originModel)
        {
            var entity = await gemStoContext.Origins.FirstOrDefaultAsync(f => f.Id == originModel.Id);

            entity.Value = originModel.Value;
            entity.Description = originModel.Description;

            gemStoContext.Entry(entity).State = EntityState.Modified;
            await gemStoContext.SaveChangesAsync();
        }
    }
}

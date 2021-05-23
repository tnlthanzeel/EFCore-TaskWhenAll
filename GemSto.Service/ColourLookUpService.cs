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
    public class ColourLookUpService : IColourLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public ColourLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<bool> AddNewColourAsync(ColourCreateModel colourCreateModel)
        {
            var isAny = await gemStoContext.Colours.AnyAsync(x => x.Value == colourCreateModel.Value && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }
            else
            {
                var entity = mapper.Map<Colour>(colourCreateModel);
                await gemStoContext.Colours.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<ColourModel>> GetAllColours()
        {
            var query = gemStoContext.Colours.Where(w => !w.IsDeleted).OrderBy(o => o.Value).AsQueryable();
            var entity = await query.AsNoTracking().ToListAsync();
            var model = mapper.Map<List<ColourModel>>(entity);
            return model;
        }

        public async Task DeleteColourAsync(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Colours] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task UpdateColourAync(ColourModel colourModel)
        {
            var entity = mapper.Map<Colour>(colourModel);
            gemStoContext.Update(entity);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<ColourModel> GetColourByIdAync(int id)
        {
            var entity = await gemStoContext.Colours.FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<ColourModel>(entity);
            return model;
        }
    }
}

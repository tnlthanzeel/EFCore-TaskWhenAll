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
    public class SellerLookUpService : ISellerLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public SellerLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }

        public async Task<bool> AddNewSeller(SellerModel sellerModel)
        {
            using (gemStoContext)
            {
                var isAny = await gemStoContext.Sellers.AnyAsync(x => x.Name == sellerModel.Name && !x.IsDeleted);
                if (isAny)
                {
                    return false;
                }
                else
                {
                    var entity = mapper.Map<Seller>(sellerModel);
                    await gemStoContext.Sellers.AddAsync(entity);
                    await gemStoContext.SaveChangesAsync();
                    return true;
                }
            }
        }

        public async Task DeleteSeller(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Sellers] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task<List<string>> GetAllSellerNamesAsync()
        {
            var sellerNames = new List<string>();
            var sellersNotInStore = await gemStoContext.Gems.Where(w => w.SellerName != null).Select(s => s.SellerName.Trim()).Distinct().ToListAsync();

            var sellersInStore = await gemStoContext.Sellers.Where(w => !w.IsDeleted).Select(s => s.Name.Trim()).ToListAsync();

            sellerNames = sellersNotInStore.Union(sellersInStore).OrderBy(name => name).ToList();
            return sellerNames;
        }

        public async Task<SellerModel> GetSellerByIdAsync(int id)
        {
            var entity = await gemStoContext.Sellers.FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<SellerModel>(entity);
            return model;
        }

        public async Task<IEnumerable<SellerModel>> GetSellerKeyValuePairAsync()
        {
            var query = gemStoContext.Sellers.Where(w => !w.IsDeleted).OrderBy(o => o.Name).AsQueryable();

            var result = await query.Select(s => new SellerModel
            {
                Id = s.Id,
                Name = s.Name,
                PhoneNumber = s.PhoneNumber
            }).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task UpdateSellerAsync(SellerModel sellerModel)
        {
            var entity = mapper.Map<Seller>(sellerModel);
            gemStoContext.Update(entity);
            await gemStoContext.SaveChangesAsync();
        }
    }
}

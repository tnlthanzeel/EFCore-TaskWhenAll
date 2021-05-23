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
    public class BuyerLookUpService : IBuyerLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public BuyerLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task CreateBuyerAsync(BuyerCreateModel buyerCreateModel)
        {
            var entity = mapper.Map<Buyer>(buyerCreateModel);
            await gemStoContext.AddAsync(entity);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task DeleteBuyerAsync(Guid id)
        {
            var entity = await gemStoContext.Buyers.FirstOrDefaultAsync(f => f.Id == id);
            entity.IsDeleted = true;
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BuyerModel>> GetAllBuyersAsync()
        {
            var listOfEntities = await gemStoContext.Buyers.Where(w => !w.IsDeleted).OrderBy(o => o.Name).ToListAsync();
            var modelList = mapper.Map<IEnumerable<BuyerModel>>(listOfEntities);

            return modelList;
        }

        public async Task<BuyerModel> GetBuyerByIdAsync(Guid id)
        {
            var entity = await gemStoContext.Buyers.FirstOrDefaultAsync(f => f.Id == id);

            var model = mapper.Map<BuyerModel>(entity);

            return model;
        }

        public async Task UpdateBuyerAsync(BuyerUpdateModel buyerUpdateModel)
        {
            var entity = mapper.Map<Buyer>(buyerUpdateModel);
            gemStoContext.Buyers.Update(entity);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task<List<string>> GetAllBuyerNamesAsync()
        {
            var buyerNames = new List<string>();
            var buyersNotInStore = await gemStoContext.GemSales.Where(w => w.BuyerName != null).Select(s => s.BuyerName.Trim()).Distinct().ToListAsync();

            var buyersInStore = await gemStoContext.Buyers.Where(w => !w.IsDeleted).Select(s => s.Name.Trim()).ToListAsync();

            buyerNames = buyersNotInStore.Union(buyersInStore).OrderBy(name => name).ToList();
            return buyerNames;
        }
    }
}

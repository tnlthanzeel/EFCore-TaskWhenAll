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
    public class CertificateProviderLookUpService : ICertificateProviderLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public CertificateProviderLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<bool> AddNewCertifiacateproviderAsync(CertificateProviderCreateModel certificateProviderCreateModel)
        {
            var isAny = await gemStoContext.CertificateProviders
                .AnyAsync(x => x.Value == certificateProviderCreateModel.Value && x.Agent == certificateProviderCreateModel.Agent && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }
            else
            {
                var entity = mapper.Map<CertificateProvider>(certificateProviderCreateModel);
                await gemStoContext.CertificateProviders.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<CertificateProviderModel>> GetAllCertificateProvidersAsync()
        {
            var query = gemStoContext.CertificateProviders.Where(w => !w.IsDeleted).OrderBy(o => o.Value).AsQueryable();

            var entity = await query.AsNoTracking().ToListAsync();

            var result = mapper.Map<IEnumerable<CertificateProviderModel>>(entity);
            return result;
        }

        public async Task DeleteCertificatProviderAsync(int id)
        {
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[CertificateProviders] SET IsDeleted = 1 WHERE Id={id}");
        }

        public async Task<CertificateProviderModel> GetCertificateProviderByidAsync(int id)
        {
            var entity = await gemStoContext.CertificateProviders.FirstOrDefaultAsync(f => f.Id == id);
            var result = mapper.Map<CertificateProviderModel>(entity);
            return result;
        }

        public async Task UpdateCertificateProvider(CertificateProviderModel certificateProviderModel)
        {
            try
            {
                var entity = mapper.Map<CertificateProvider>(certificateProviderModel);
                gemStoContext.Update(entity);
                await gemStoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}

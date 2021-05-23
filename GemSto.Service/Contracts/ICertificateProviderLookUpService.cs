using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ICertificateProviderLookUpService
    {
        Task<bool> AddNewCertifiacateproviderAsync(CertificateProviderCreateModel certificateProviderCreateModel);
        Task<IEnumerable<CertificateProviderModel>> GetAllCertificateProvidersAsync();
        Task DeleteCertificatProviderAsync(int id);
        Task<CertificateProviderModel> GetCertificateProviderByidAsync(int id);
        Task UpdateCertificateProvider(CertificateProviderModel certificateProviderModel);
    }
}

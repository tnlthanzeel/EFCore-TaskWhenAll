using GemSto.Common;
using GemSto.Service.Models;
using GemSto.Service.Models.Certification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ICertificateService
    {
        Task<bool> CreateAsync(CertificateCreateModel certificateCreateModel, decimal? length = null, decimal? width = null, decimal? depth = null, int? recutShapeId = null, decimal? recutWeight = null);
        Task DeleteAsync(int id, string editedById, string editedByName);
        Task<List<CertificateModel>> GetCertificatesByGemId(int id);
        Task<CertificateModel> GetByIdAsync(int certificateId);
        Task UpdateCertificateDetailAsync(CertificateUpdateModel certificateModel);
        Task ChangeDefaultCertificateAsync(int gemId, int certificateId, string editedeById, string editedByName);
        Task AddToCertificationAsync(CertificationCreateModel certificationCreateModel);
        Task RemoveGemFromCertificationAsync(int id, string editedById, string editedByName);
        Task<PaginationModel<CertificationModel>> GetAllGemToCertificationAsync(PaginationBase paginationBase, CertificationFilterModel certificationFilterModel);
        Task AddCertificatonCertificateAsync(CertificationCertificateCreateModel certificationCertificateCreateModel);
        Task UpdateCertificationAsync(CertificationUpdateModel certificationUpdateModel);
        Task UpdateThirdPartyCertificationAsync(ThirdPartyCertificationUpdateModel thirdPartyCertificationUpdateModel);
        Task<CertificationModel> GetThirdPartyCertificationByIdAsync(int id);
        Task AddThirdPartyCertificateAsync(AddThirdPartyCertificateModel addThirdPartyCertificateModel);
        Task<ThirdPartyCertificateModel> GetThirdCertificateByIdAsync(int certificationId);
    }
}

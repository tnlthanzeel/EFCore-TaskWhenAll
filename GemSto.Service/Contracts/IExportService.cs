using GemSto.Common;
using GemSto.Service.Models.Export;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IExportService
    {
        Task<string> CreateExport(ExportCreateModel exportCreateModel);
        Task GetExportById(string exportId);
        Task<PaginationModel<ExportSummaryModel>> GetAllExports(PaginationBase paginationBase);
        Task GetExportDetailById(Guid exportId);
        Task<int> GetLastExprtIdAsync();
        Task<ExportCreateModel> GetExportHeaderById(string exportId);
        Task UpdateExportSummaryAync(ExportCreateModel exportCreateModel);
        Task AddGemToExportAsync(GemToExport gemToExport);
        Task<PaginationModel<ExportDetailModel>> GetExportDetailByIdAsync(PaginationBase paginationBase, Guid exportId);
        Task AddThirdPartyGemToExportAsync(ExportThirdPartyCreateModel exportThirdPartyCreateModel);
        Task RemoveGemFromExportAsync(int exportId, string editedById, string editedByName);
        Task<ExportDetailModel> GetExportDetailByIdAsync(int gemExportId);
        Task UpdateThridPartyGemAsync(ThirdPartyUpdateModel thirdPartyUpdateModel);
        Task ChangeStockIdForExportAsync(int id, int gemId, string editedById, string editedByName);
        Task CloseExportAsync(Guid exportId, string editedById, string editedByName);
        Task DeleteExport(Guid exportId, string editedBy, string editedByName);
    }
}

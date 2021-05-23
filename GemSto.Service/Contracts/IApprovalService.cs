using GemSto.Common;
using GemSto.Service.Models.Approval;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IApprovalService
    {
        Task<string> CreateApprovalSummaryAsync(ApprovalSummaryCreateModel approvalSummaryCreateModel);
        Task CloseApprovalAsync(Guid approvalId, string editedById, string editedByName);
        Task<PaginationModel<ApprovalSummaryModel>> GetAllApprovalSummaryAsync(PaginationBase paginationBase);
        Task ApprovalSummaryUpdateAsync(ApprovalSummaryUpdateModel approvalSummaryUpdateModel);
        Task AddGemToApprovalAsync(AddGemToApprovalListModel addGemToApprovalListModel);
        Task<PaginationModel<ApprovalGemModel>> GetApprovalDetailByIdAsync(PaginationBase paginationBase, Guid approvalId, bool isPrintable, bool isAll);
        Task<ApprovalSummaryModel> GetApprovalSummaryByIdIdAsync(Guid approvalId);
        Task RemoveGemFromApprovalAsync(int id, string editedById,string editedByName);
        Task EditApprivalDetailByIdAsync(ApprovalDetailUpdateModel approvalDetailUpdateModel, string editedById, string editedByName);
        Task<ApprovalGemModel> GetApprovalGemByIdAsync(int approvalDetailId);
    }
}

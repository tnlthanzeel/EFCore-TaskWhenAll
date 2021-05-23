using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Service.Models.Approver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IApproverLookUpService
    {
        Task<bool> AddNewApproverAsync(ApproverCreateModel approverCreateModel);
        Task UpdatApproverAsync(ApproverUpdateModel approverUpdateModel);
        Task<Guid?> DeleteApproverAsync(Guid approverId);
        Task<IEnumerable<ApproverModel>> GetAllApproversAsync(ApproverFilter approverFilter);
        Task<ApproverModel> GetApproverByIdAsync(Guid id);
        Task ChangeApproverProfileStatusAsync(StatusChangeModel<Guid> statusChangeModel);
    }
}

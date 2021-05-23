using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain.LookUp;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Approver;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class ApproverLookUpService : IApproverLookUpService
    {
        private readonly GemStoContext gemStoContext;
        private readonly IMapper mapper;

        public ApproverLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            this.gemStoContext = gemStoContext;
            this.mapper = mapper;
        }
        public async Task<bool> AddNewApproverAsync(ApproverCreateModel approverCreateModel)
        {
            var isApproverAvailable = await gemStoContext.Approvers.AnyAsync(a => a.Name.ToLower() == approverCreateModel.Name.ToLower() && !a.IsDeleted);
            if (isApproverAvailable)
            {
                return false;
            }
            else
            {
                var entity = mapper.Map<Approver>(approverCreateModel);
                await gemStoContext.AddAsync(entity);
                await gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Guid?> DeleteApproverAsync(Guid approverId)
        {
            var approval = await gemStoContext.Approvals.FirstOrDefaultAsync(f => f.ApproverId == approverId);
            await gemStoContext.Database.ExecuteSqlCommandAsync($"UPDATE [dbo].[Approvers] SET IsDeleted = 1 WHERE Id={approverId}");
            return approval?.Id;
        }

        public async Task<IEnumerable<ApproverModel>> GetAllApproversAsync(ApproverFilter approverFilter)
        {
            var query = gemStoContext.Approvers.Where(w => !w.IsDeleted);

            if (approverFilter == ApproverFilter.WihtoutProfile)
            {
                query = query.Where(w => !w.IsProfileCreated);
            }

            var entity = await query.AsNoTracking().ToListAsync();

            var model = mapper.Map<IEnumerable<ApproverModel>>(entity);
            return model;
        }

        public async Task<ApproverModel> GetApproverByIdAsync(Guid id)
        {
            var entity = await gemStoContext.Approvers.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            var model = mapper.Map<ApproverModel>(entity);
            return model;
        }

        public async Task UpdatApproverAsync(ApproverUpdateModel approverUpdateModel)
        {
            var entity = mapper.Map<Approver>(approverUpdateModel);
            gemStoContext.Update(entity);
            await gemStoContext.SaveChangesAsync();
        }

        public async Task ChangeApproverProfileStatusAsync(StatusChangeModel<Guid> statusChangeModel)
        {
            var entity = await gemStoContext.Approvers.FirstOrDefaultAsync(f => f.Id == statusChangeModel.Id);
            entity.IsProfileCreated = statusChangeModel.Status;
            //await gemStoContext.SaveChangesAsync();
        }
    }
}

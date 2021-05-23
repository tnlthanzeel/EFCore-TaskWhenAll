using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class AddGemToApprovalListModel : ModelBase
    {
        public int GemId { get; set; }
        public Guid ApprovalId { get; set; }
    }
}

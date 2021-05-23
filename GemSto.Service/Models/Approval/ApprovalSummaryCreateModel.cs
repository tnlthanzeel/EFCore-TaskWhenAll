using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class ApprovalSummaryCreateModel: ModelBase
    {
        public int ApprovalNumber { get; set; }
        public string Description { get; set; }
        public Guid ApproverId { get; set; }
    }
}

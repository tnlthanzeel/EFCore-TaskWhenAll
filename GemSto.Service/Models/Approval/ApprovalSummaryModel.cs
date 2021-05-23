using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class ApprovalSummaryModel
    {
        public Guid Id { get; set; }
        public string ApproverName { get; set; }
        public string Description { get; set; }
        public int NumberOfPieces { get; set; }
        public Guid ApproverId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class ApprovalSummaryUpdateModel: ModelBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string ApproverName { get; set; }
        public string Description { get; set; }
    }
}

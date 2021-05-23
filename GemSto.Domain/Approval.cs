using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class Approval : DomainBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        [StringLength(4000)]
        public string Description { get; set; }
        public ICollection<GemApproval> GemApprovals { get; set; }
        public Approver Approver { get; set; }
        public Guid ApproverId { get; set; }
    }
}

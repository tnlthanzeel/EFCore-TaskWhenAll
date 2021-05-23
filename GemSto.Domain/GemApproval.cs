using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GemSto.Domain
{
    public class GemApproval : DomainBase
    {
        public int Id { get; set; }
        public int GemId { get; set; }
        public Gem Gem { get; set; }
        public Guid ApprovalId { get; set; }
        public Approval Approval { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        [StringLength(4000)]
        public string Description { get; set; }
    }
}

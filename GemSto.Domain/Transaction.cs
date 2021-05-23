using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class Transaction: DomainBase
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? PaidOn { get; set; }
        public decimal PaidAmount { get; set; }
        public int GemId { get; set; }
        public Gem Gem { get; set; }
        public GemStatus? GemStatus { get; set; }
        public Remark Remark { get; set; } = Remark.Created;
        public TransactionType TransactionType { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}

using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Domain
{
    public class MiscellaneousPayments : DomainBase
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset PaidOn { get; set; }
        public Miscellaneous Miscellaneous { get; set; }
        public int MiscellaneousId { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    }
}

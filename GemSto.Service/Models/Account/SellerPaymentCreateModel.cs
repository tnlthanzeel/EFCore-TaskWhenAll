using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Account
{
    public class SellerPaymentCreateModel
    {
        public int GemId { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTimeOffset PaidOn { get; set; } = DateTimeOffset.UtcNow;
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Miscellaneous
{
    public class MiscPaymentUpdateModel
    {
        public long Id { get; set; }
        public DateTimeOffset PaidOn { get; set; }
        public string Description { get; set; }
        public decimal PaidAmount { get; set; }
    }
}

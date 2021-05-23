using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class SellerPaymentModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? EditedOn { get; set; }
        public string Description { get; set; }
    }
}

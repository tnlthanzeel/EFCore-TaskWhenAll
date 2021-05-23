using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class SellerLotPayment : DomainBase
    {
        public long Id { get; set; }
        [MaxLength(1000)]
        public string GemIdsAsString { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

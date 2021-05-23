using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class BuyerLotPayment : DomainBase
    {

        public long Id { get; set; }
        [MaxLength(1000)]
        public string GemSaleIdsAsString { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

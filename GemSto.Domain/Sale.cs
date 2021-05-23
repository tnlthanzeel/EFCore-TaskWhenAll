using GemSto.Common.Enum;
using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class Sale : DomainBase
    {
        public Sale()
        {
            GemSales = new List<GemSales>();
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public List<GemSales> GemSales { get; set; }
        public List<SalePayment> SalePayments { get; set; }
        public int Number { get; set; }
        public decimal? Commission { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
    }
}

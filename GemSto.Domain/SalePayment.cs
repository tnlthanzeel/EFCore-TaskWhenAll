using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Domain
{
    public class SalePayment : DomainBase
    {
        public int Id { get; set; }
        public int GemSalesId { get; set; }
        public GemSales GemSales { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public decimal AmmountPaid { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsPaymentApproved { get; set; }
        public Guid? SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}

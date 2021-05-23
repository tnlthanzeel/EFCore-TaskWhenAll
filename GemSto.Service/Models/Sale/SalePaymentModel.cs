using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SalePaymentModel
    {
        public int Id { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal AmmountPaid { get; set; }
        public bool IsPaymentApproved { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}

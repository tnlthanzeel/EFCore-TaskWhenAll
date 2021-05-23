using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class SellerPaymentHistoryModel
    {
        public long Id { get; set; }
        public string SellerName { get; set; }
        public string StockNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTimeOffset PaidOn { get; set; }
        public bool IsSinglePurchase { get; set; }
        public int GemId { get; set; }
        public Guid GemIdentity { get; set; }
        public PaymentHistoryType PaymentHistoryType { get; set; }
        public bool IsThirdParty { get; set; } = false;
        public string Description { get; set; }
        public string GemStockNumber { get; set; }
        public int SaleId { get; set; }

    }
}

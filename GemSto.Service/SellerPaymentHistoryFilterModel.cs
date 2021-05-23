using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service
{
    public class SellerPaymentHistoryFilterModel
    {
        public string SearchQuery { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public decimal? MaxAmount { get; set; }
        public decimal? MinAmount { get; set; }
        public string Participants { get; set; }
        public List<PaymentHistoryType> PaymentType { get; set; } = new List<PaymentHistoryType>();
        public int? PrimaryMiscCat { get; set; }
        public int? SubMiscCat { get; set; }
        public string SaleId { get; set; }
        public int? StockId { get; set; }
        public bool IsGlobalSearch { get; set; }
    }
}

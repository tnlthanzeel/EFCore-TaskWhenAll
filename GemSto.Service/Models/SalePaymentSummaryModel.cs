using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class SalePaymentSummaryModel
    {
        public string BuyerName { get; set; }
        public DateTimeOffset SaleDate { get; set; }
        public decimal? Price { get; set; }
        public decimal SellingRate { get; set; }
        public string StockNumber { get; set; }
        public decimal Weight { get; set; }
        public string SaleNumber { get; set; }
    }
}

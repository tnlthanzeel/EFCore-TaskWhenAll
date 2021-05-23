using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SaleGemsWithCertificatePendingModel
    {
        public string SaleNumber { get; set; }
        public string StockNumber { get; set; }
        public decimal? Weight { get; set; }
        public string Shape { get; set; }
        public string SaleDate { get; set; }
        public string Variety { get; set; }
        public decimal Rate { get; set; }
        public decimal? BuyerCurrency { get; set; }
        public decimal AmountPaid { get; set; }
        public bool IsThirdParty { get; set; }
    }
}

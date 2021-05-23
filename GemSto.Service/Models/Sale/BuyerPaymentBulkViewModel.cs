using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class BuyerPaymentBulkViewModel
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? TotalAmount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal TotalAmountPaid { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal? TotalAmountPayable { get; set; }
        public bool IsThirdParty { get; set; }
        public bool IsSingleSale { get; set; }
        public string ImageUrl { get; set; }
        public string Variety { get; set; }
        public bool? Treatment { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Rate { get; set; }
    }
}

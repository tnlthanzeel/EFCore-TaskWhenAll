using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class LastFivePaymentsModel
    {
        public string SaleNumber { get; set; }
        public string Date { get; set; }
        public decimal Rate { get; set; }
        public decimal AmountPaid { get; set; }
    }
}

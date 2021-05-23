using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class BuyerPayableModel
    {
        public decimal TotalCommission { get; set; }
        public decimal TotalCP { get; set; }
        public decimal TotalWC { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalPCAmount { get; set; }
        public decimal TotalOfGrossTotal { get; set; }
    }
}

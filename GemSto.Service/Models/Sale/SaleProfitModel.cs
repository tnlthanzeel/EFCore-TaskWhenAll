using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SaleProfitModel : SaleModel
    {
        public decimal? GemTotalCost { get; set; }
        public Guid? SaleIdGuid { get; set; }
        public decimal? LotProfit { get; set; }
    }
}


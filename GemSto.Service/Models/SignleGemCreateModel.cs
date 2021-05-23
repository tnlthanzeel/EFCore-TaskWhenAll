using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class SignleGemCreateModel : ModelBase
    {
        public string StockNumber { get; set; }
        public decimal InitialCost { get; set; }
        public decimal InitialWeight { get; set; }
        public bool IsTreated { get; set; }
        public ShapeModel Shape { get; set; }
        public VarietyModel Variety { get; set; }
        public SellerModel Seller { get; set; }
        public string SellerName { get; set; }
        public decimal TotalAmountPaidToSeller { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public decimal? BrokerFee { get; set; }
        public string Note { get; set; }
        public DateTimeOffset? PurchasedDate { get; set; }
        public decimal Share { get; set; } = 1.00m;
    }
}

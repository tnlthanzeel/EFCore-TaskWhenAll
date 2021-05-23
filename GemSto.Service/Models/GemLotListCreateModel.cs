using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class GemLotListCreateModel : ModelBase
    {
        public GemLotListCreateModel()
        {
            this.LotList = new List<GemLotCreateList>();
        }

        public string StockNumber { get; set; }
        public SellerModel Seller { get; set; }
        public string SellerName { get; set; }
        public List<GemLotCreateList> LotList { get; set; }
        public decimal GemLotCost { get; set; }
        public decimal TotalAmountPaidToSeller { get; set; }
        public decimal? BrokerFee { get; set; }
        public string Note { get; set; }
        public DateTimeOffset? PurchasedDate { get; set; }
        public Guid Identity { get; set; }
        public decimal Share { get; set; } = 1.00m;
    }
}

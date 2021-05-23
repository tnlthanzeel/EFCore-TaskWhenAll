using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class ThirdPartySaleUpdateModel
    {
        public int Id { get; set; }
        public decimal SellingRate { get; set; }
        public decimal Price { get; set; }
        public Guid? BuyerId { get; set; }
        public DateTimeOffset SaleDate { get; set; }
        public bool? IsCertificatePending { get; set; }
        public string BuyerName { get; set; }
        public decimal? Commission { get; set; }
        public int VarietyId { get; set; }
        public int ShapeId { get; set; }
        public decimal Weight { get; set; }
        public bool IsTreated { get; set; }
        public string Owner { get; set; }
        public string Note { get; set; }
    }
}

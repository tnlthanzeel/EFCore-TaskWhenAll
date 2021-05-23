using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SaleCreateModel : ModelBase
    {
        public SaleCreateModel()
        {
            this.GemSales = new List<GemSaleCreateModel>();
        }
        public Guid? BuyerId { get; set; }
        public List<GemSaleCreateModel> GemSales { get; set; }
        public decimal AmmountPaid { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal SellingRate { get; set; }
        public int SaleNumber { get; set; }
        public bool IsSingleSale { get; set; }
        public decimal? GrossTotal { get; set; }
        public bool IsThirdParty { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal? Weight { get; set; }
        public int? ShapeId { get; set; }
        public int? VarietyId { get; set; }
        public string ThirdPartyOwner { get; set; }
        public bool? IsTreated { get; set; }
        public string BuyerName { get; set; }
        public decimal? Commission { get; set; }
        public int? GemExportId { get; set; }
        public string Note { get; set; }
    }
}

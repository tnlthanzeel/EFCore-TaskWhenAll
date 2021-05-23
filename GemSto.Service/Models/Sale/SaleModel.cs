using GemSto.Common.Enum;
using GemSto.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SaleModel
    {
        public int Id { get; set; }
        public string SaleId { get; set; }
        public string Variety { get; set; }
        public string StockNo { get; set; }
        public bool? IsTreated { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Buyer { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalAmountReceived { get; set; }
        public int NoOfPieces { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal SellingRate { get; set; }
        public decimal? Weight { get; set; }
        public bool? IsCertificatePending { get; set; }
        public bool IsSingleSale { get; set; }
        public bool IsThirdParty { get; set; }
        public decimal? Share { get; set; }
        public string Shape { get; set; }
        public bool IsAllPaymentsApproved { get; set; }
        public Guid? BuyerId { get; set; }
        public decimal? Commission { get; set; }
        public int? ShapeId { get; set; }
        public int? VarietyId { get; set; }
        public Guid? GemIdentity { get; set; }
        public int? GemId { get; set; }
        public bool IsDeleted { get; set; }
        public string Note { get; set; }
        public decimal TotalAmountPaid { get; set; }
    }
}

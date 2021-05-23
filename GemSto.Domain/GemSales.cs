using GemSto.Common.Enum;
using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GemSto.Domain
{
    public class GemSales : DomainBase
    {
        public int Id { get; set; }
        public Sale Sale { get; set; }
        public Guid? SaleId { get; set; }
        public int? GemId { get; set; }
        public Gem Gem { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public decimal? TotalAmount { get; set; }
        public bool? IsCertificatePending { get; set; }
        [StringLength(200)]
        public string SaleNumber { get; set; }
        public decimal SellingRate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public bool IsSingleSale { get; set; }
        public bool IsThirdParty { get; set; }
        public decimal? Weight { get; set; }
        public Buyer Buyer { get; set; }
        public Guid? BuyerId { get; set; }
        public int NumberOfPieces { get; set; }
        public int? ShapeId { get; set; }
        public Shape Shape { get; set; }
        public int? VarietyId { get; set; }
        public Variety Variety { get; set; }
        [StringLength(200)]
        public string ThirdPartyOwner { get; set; }
        public bool? IsTreated { get; set; }
        public List<SalePayment> SalePayments { get; set; }
        [StringLength(200)]
        public string BuyerName { get; set; }
        public GemExport GemExport { get; set; }
        public int? GemExportId { get; set; }

        public int? GemApprovalId { get; set; }
        [ForeignKey(nameof(GemApprovalId))]
        public GemApproval GemApproval { get; set; }
    }
}

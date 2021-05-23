using GemSto.Common.Enum;
using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GemSto.Domain
{
    public class Gem : DomainBase
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string StockNumber { get; set; }
        public decimal InitialCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal InitialWeight { get; set; }
        public decimal RecutWeight { get; set; }
        public bool? IsTreated { get; set; }
        public int? SellerId { get; set; }
        public Seller Seller { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
        public int? ShapeId { get; set; }
        public Shape Shape { get; set; }
        public int? VarietyId { get; set; }
        public Variety Variety { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<GemExport> GemExports { get; set; }
        [StringLength(200)]
        public string SellerName { get; set; }
        public PaymentStatus PaymentStatusToSeller { get; set; }
        public GemStatus GemStatus { get; set; }
        public decimal TotalAmountPaidToSeller { get; set; }
        public DateTimeOffset? PurchasedDate { get; set; } = DateTimeOffset.UtcNow;
        public int? GemLotId { get; set; }
        public GemLot GemLot { get; set; }
        public int NumberOfPieces { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }
        public decimal BrokerFee { get; set; }
        public bool IsGemLot { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        public Guid Identity { get; set; } = Guid.NewGuid();
        public int? RecutShapeId { get; set; }
        [ForeignKey("RecutShapeId")]
        public virtual Shape RecutShape { get; set; }
        [Column(TypeName = "decimal(18,5)")]
        public decimal Share { get; set; } = 1.00m;
        public decimal? SellingPrice { get; set; }
        public bool IsThirdParty { get; set; }
    }
}

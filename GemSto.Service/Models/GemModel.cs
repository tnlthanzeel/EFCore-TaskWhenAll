using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Domain;
using GemSto.Domain.LookUp;
using GemSto.Service.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models
{
    public class GemModel : PaginationModel<GemModel>
    {
        public int Id { get; set; }
        public string StockNumber { get; set; }
        public decimal InitialCost { get; set; }
        public decimal TotalCost { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal InitialWeight { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal RecutWeight { get; set; }
        public bool? IsTreated { get; set; }
        public bool IsDeleted { get; set; }
        public ShapeModel Shape { get; set; }
        public VarietyModel Variety { get; set; }
        public string SellerName { get; set; }
        public PaymentStatus PaymentStatusToSeller { get; set; }
        public GemStatus GemStatus { get; set; }
        public decimal TotalAmountPaidToSeller { get; set; }
        public DateTimeOffset? PurchasedDate { get; set; }
        public DateTimeOffset? PaidToSellerOn { get; set; }
        public SellerModel Seller { get; set; }
        public int NumberOfPieces { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Length { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Width { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Depth { get; set; }
        public decimal BrokerFee { get; set; }
        public bool IsGemLot { get; set; }
        public string Note { get; set; }
        public Guid Identity { get; set; }
        public ShapeModel RecutShape { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Share { get; set; }
        public int? GemLotId { get; set; }
        public decimal? SellingPrice { get; set; }

    }
}

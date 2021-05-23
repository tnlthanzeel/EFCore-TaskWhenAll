using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class GlobalSearchFilterModel
    {
        public IEnumerable<int?> CertificateProviderId { get; set; } = null;
        public IEnumerable<int?> ColorId { get; set; } = null;
        public decimal? MaxCost { get; set; } = null;
        public decimal? MaxWeight { get; set; } = null;
        public decimal? MinCost { get; set; } = null;
        public decimal? MinWeight { get; set; } = null;
        public IEnumerable<PaymentStatus?> PaymentStatus { get; set; } = null;
        public IEnumerable<GemStatus?> GemStatus { get; set; } = null;

        public string Seller { get; set; } = null;
        public IEnumerable<int?> ShapeId { get; set; } = null;
        public IEnumerable<int?> VarietyId { get; set; } = null;
        public string CertificateNumber { get; set; } = null;
        public decimal? Length { get; set; } = null;
        public decimal? Width { get; set; } = null;
        public decimal? Depth { get; set; } = null;
        public bool? IsHeated { get; set; } = null;
        public bool? IsNatural { get; set; } = null;
        public DateTimeOffset? PurchasedFrom { get; set; } = null;
        public DateTimeOffset? PurchasedTo { get; set; } = null;

    }
}

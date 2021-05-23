using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class SaleGlobalSearchFilterModel
    {
        public IEnumerable<int?> CertificateProviderId { get; set; } = null;
        public decimal? MaxCost { get; set; } = null;
        public decimal? MaxWeight { get; set; } = null;
        public decimal? MinCost { get; set; } = null;
        public decimal? MinWeight { get; set; } = null;
        public IEnumerable<PaymentStatus?> PaymentStatus { get; set; } = null;
        public string Buyer { get; set; } = null;
        public IEnumerable<int?> ShapeId { get; set; } = null;
        public IEnumerable<int?> VarietyId { get; set; } = null;
        public string CertificateNumber { get; set; } = null;
        public bool? IsHeated { get; set; } = null;
        public bool? IsNatural { get; set; } = null;
        public DateTimeOffset? SaleFrom { get; set; } = null;
        public DateTimeOffset? SaleTo { get; set; } = null;
        public bool IsThirdParty { get; set; }
        public bool IsCertificatePending { get; set; }
    }
}

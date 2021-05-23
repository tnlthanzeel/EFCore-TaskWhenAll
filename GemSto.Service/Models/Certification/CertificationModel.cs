using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class CertificationModel
    {
        public int Id { get; set; }
        public string CertificateProviderName { get; set; }
        public int CertificateProviderId { get; set; }
        public string StockNumber { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public DateTimeOffset? ReceivedDate { get; set; }
        public bool IsCertified { get; set; }
        public Guid? GemIdentity { get; set; }
        public decimal Weight { get; set; }
        public bool IsThirdParty { get; set; }
        public string ShapePath { get; set; }
        public bool? IsTreated { get; set; }
        public string Variety { get; set; }
        public int? VarietyId { get; set; }
        public int? GemId { get; set; }
        public string CertProviderAgent { get; set; }
    }
}

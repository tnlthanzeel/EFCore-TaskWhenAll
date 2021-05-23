using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class Certification : DomainBase
    {
        public int Id { get; set; }
        public int? GemId { get; set; }
        public Gem Gem { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public bool IsCertified { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public DateTimeOffset? RecievedDate { get; set; }
        public CertificateProvider CertificateProvider { get; set; }
        public int CertificateProviderId { get; set; }
        public bool IsThirdParty { get; set; }
        public decimal? Weight { get; set; }
        [StringLength(300)]
        public string Owner { get; set; }
        public ThirdPartyCertificate ThirdPartyCertificate { get; set; }
        public Shape Shape { get; set; }
        public int? ShapeId { get; set; }
        public bool? IsTreated { get; set; }
        public Variety Variety { get; set; }
        public int? VarietyId { get; set; }
    }
}

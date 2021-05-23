using GemSto.Common.Enum;
using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class GemExport : DomainBase
    {
        public int Id { get; set; }
        public int? GemId { get; set; }
        public Gem Gem { get; set; }
        public Guid ExportId { get; set; }
        public Export Export { get; set; }
        public Variety Variety { get; set; }
        public int? VarietyId { get; set; }
        public decimal? Weight { get; set; }
        [StringLength(200)]
        public string Owner { get; set; }
        public Shape Shape { get; set; }
        public int? ShapeId { get; set; }
        public CertificateProvider CertificateProvider { get; set; }
        public int? CertificateProviderId { get; set; }
        public Colour Colour { get; set; }
        public int? ColourId { get; set; }
        [StringLength(200)]
        public string Cost { get; set; }
        public bool? IsTreated { get; set; }
        public int? OriginId { get; set; }
        public Origin Origin { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public GemStatus? GemStatus { get; set; }
        public bool IsThirdParty { get; set; }
        public int Number { get; set; }
    }
}

using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Export
{
    public class ThirdPartyUpdateModel : ModelBase
    {
        public int Id { get; set; }
        public int? VarietyId { get; set; }
        public decimal? Weight { get; set; }
        public string Owner { get; set; }
        public int? ShapeId { get; set; }
        public int? CertificateProviderId { get; set; }
        public int? ColourId { get; set; }
        public string Cost { get; set; }
        public bool IsTreated { get; set; }
        public int? OriginId { get; set; }
        public GemStatus GemStatus { get; set; } = GemStatus.Exported;
        public Guid ExportId { get; set; }
    }
}

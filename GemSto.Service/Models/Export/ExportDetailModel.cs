using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Export
{
    public class ExportDetailModel
    {
        public int GemExportId { get; set; }
        public string variety { get; set; }
        public decimal Weight { get; set; }
        public string Owner { get; set; }
        public string Shape { get; set; }
        public string Certificate { get; set; }
        public string ColorValue { get; set; }
        public string OriginValue { get; set; }
        public string Cost { get; set; }
        public bool IsTreated { get; set; }
        public GemStatus GemStatus { get; set; }
        public bool IsThirdParty { get; set; }
        public int? VarietyId { get; set; }
        public int? ShapeId { get; set; }
        public int? CertificateProviderId { get; set; }
        public int? ColourId { get; set; }
        public int? OriginId { get; set; }
        public int Number { get; set; }
        public Guid ExportId { get; set; }
        public Guid? GemIdentity { get; set; }
        public int? GemId { get; set; }
    }
}

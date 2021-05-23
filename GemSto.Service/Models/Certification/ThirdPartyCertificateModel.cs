using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class ThirdPartyCertificateModel
    {
        public string CerticateProvider { get; set; }
        public string CerticateNumber { get; set; }
        public string Color { get; set; }
        public decimal CertificateFee { get; set; }
        public bool IsTreated { get; set; }
        public string OriginName { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public decimal? Weight { get; set; }
        public string ShapeName { get; set; }
        public string ShapePath { get; set; }
        public string VarietyName { get; set; }

    }
}

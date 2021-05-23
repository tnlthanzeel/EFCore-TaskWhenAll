using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class AddThirdPartyCertificateModel
    {
        public int ColourId { get; set; }
        public int? OriginId { get; set; }
        public bool IsTreated { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public int ShapeId { get; set; }
        public string Number { get; set; }
        public decimal? Cost { get; set; }
        public int CertificationId { get; set; }
        public decimal? Weight { get; set; }
    }
}

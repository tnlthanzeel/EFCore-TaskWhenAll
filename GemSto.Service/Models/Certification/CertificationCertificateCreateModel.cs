using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class CertificationCertificateCreateModel : ModelBase
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ColourId { get; set; }
        public decimal CertificateFee { get; set; }
        public bool IsTreated { get; set; }
        public int? OriginId { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public int? RecutShapeId { get; set; }
        public decimal? RecutWeight { get; set; }
        public bool IsThirdParty { get; set; }
        public decimal? Weight { get; set; }
        public string CertURL { get; set; }
    }
}

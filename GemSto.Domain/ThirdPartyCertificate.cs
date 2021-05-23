using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class ThirdPartyCertificate : DomainBase
    {
        public int Id { get; set; }
        public int ColourId { get; set; }
        public Colour Colour { get; set; }
        public int? OriginId { get; set; }
        public Origin Origin { get; set; }
        public bool IsTreated { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public int ShapeId { get; set; }
        public Shape Shape { get; set; }
        [StringLength(1000)]
        public string Number { get; set; }
        public decimal Cost { get; set; }
        public Certification Certification { get; set; }
        public int CertificationId { get; set; }
        public decimal? Weight { get; set; }
    }
}

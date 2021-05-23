using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class CertificateUpdateModel : ModelBase
    {
        public int Id { get; set; }
        public int CertificateproviderId { get; set; }
        public string CerticateNumber { get; set; }
        public int ColourId { get; set; }
        public decimal CertificateFee { get; set; }
        public bool IsTreated { get; set; }
        public int? OriginId { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public string CertURL { get; set; }
    }
}

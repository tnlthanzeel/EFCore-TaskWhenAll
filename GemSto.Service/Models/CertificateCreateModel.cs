using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class CertificateCreateModel : ModelBase
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int CertificateProviderId { get; set; }
        public int GemId { get; set; }
        public bool IsDeleted { get; set; }
        public int ColourId { get; set; }
        public decimal CertificateFee { get; set; }
        public bool IsTreated { get; set; }
        public int? OriginId { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public string CertURL { get; set; }
    }
}

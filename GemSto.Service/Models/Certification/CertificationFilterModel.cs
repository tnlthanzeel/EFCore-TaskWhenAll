using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class CertificationFilterModel
    {
        public bool ShowAll { get; set; }

        public int? CertificateProviderId { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public decimal? MaxWeight { get; set; }
        public decimal? MinWeight { get; set; }
        public bool IsPrintable { get; set; }
    }
}

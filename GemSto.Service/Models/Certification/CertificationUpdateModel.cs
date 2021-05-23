using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class CertificationUpdateModel : ModelBase
    {
        public int Id { get; set; }
        public int CertificateProviderId { get; set; }
        public int? GemId { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Certification
{
    public class ThirdPartyCertificationUpdateModel
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }
        public string Owner { get; set; }
        public int CertificateProviderId { get; set; }
        public DateTimeOffset SubmissionDate { get; set; }
        public int? ShapeId { get; set; }
        public bool IsTreated { get; set; }
        public int VarietyId { get; set; }

    }
}

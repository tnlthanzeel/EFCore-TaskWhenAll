using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models
{
    public class CertificateProviderCreateModel
    {
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        public string Agent { get; set; }

    }
}

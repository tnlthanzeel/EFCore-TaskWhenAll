using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class CertificateProviderModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Agent { get; set; }

    }
}

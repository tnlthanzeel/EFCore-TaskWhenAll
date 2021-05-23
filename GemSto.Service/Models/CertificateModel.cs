using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class CertificateModel
    {
        public CertificateModel()
        {
            this.Color = new ColourModel();
        }
        public int Id { get; set; }
        public string CerticateProvider { get; set; }
        public string CerticateNumber { get; set; }
        public ColourModel Color { get; set; }
        public decimal CertificateFee { get; set; }
        public int CertificateproviderId { get; set; }
        public int ColourId { get; set; }
        public bool IsTreated { get; set; }
        public int? OriginId { get; set; }
        public string OriginName { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        public string Description { get; set; }
        public string CertProviderAgent { get; set; }
        public string CertURL { get; set; }
    }
}

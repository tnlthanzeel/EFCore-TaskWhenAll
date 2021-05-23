using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public class Certificate
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Number { get; set; }
        public int CertificateProviderId { get; set; }
        public CertificateProvider CertificateProvider { get; set; }
        public int GemId { get; set; }
        public Gem Gem { get; set; }
        public bool IsDeleted { get; set; }
        public int ColourId { get; set; }
        public Colour Colour { get; set; }
        public int? TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public int? OriginId { get; set; }
        public Origin Origin { get; set; }
        public bool IsTreated { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset? CertifiedDate { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [MaxLength(8000)]
        public string CertURL { get; set; }
    }
}

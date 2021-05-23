using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.LookUp
{
    public class Buyer: DomainBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required,StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string PhoneNumber { get; set; }
    }
}

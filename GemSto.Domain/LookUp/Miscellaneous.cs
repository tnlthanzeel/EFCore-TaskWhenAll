using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.LookUp
{
    public class Miscellaneous : DomainBase
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string PaidTo { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain
{
    public abstract class DomainBase
    {
        [StringLength(100)]
        public string CreatedById { get; set; }
        [StringLength(100)]
        public string EditedById { get; set; }
        public DateTimeOffset? EditedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

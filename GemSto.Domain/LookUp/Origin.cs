using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.LookUp
{
    public class Origin
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string Value { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}

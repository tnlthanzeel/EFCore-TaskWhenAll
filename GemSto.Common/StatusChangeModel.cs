using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Common
{
    public class StatusChangeModel<T>
    {
        [Required]
        public T Id { get; set; }
        public bool Status { get; set; }
    }
}

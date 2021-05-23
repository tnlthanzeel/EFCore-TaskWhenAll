using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.LookUp
{
    public class Seller
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
    }
}

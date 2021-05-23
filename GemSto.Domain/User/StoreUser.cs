using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.User
{
    public class StoreUser : IdentityUser
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset DOB { get; set; }

        public string TelephoneLineTwo { get; set; }

        [Required, StringLength(4000)]
        public string Address { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }
    }
}

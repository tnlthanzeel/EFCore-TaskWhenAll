using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models
{
    public class StaffModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTimeOffset DOB { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string TelephoneLineTwo { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

    }
}

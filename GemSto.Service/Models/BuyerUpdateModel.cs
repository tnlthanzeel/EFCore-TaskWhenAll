﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class BuyerUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}

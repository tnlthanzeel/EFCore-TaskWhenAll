using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GemSto.Service.Models
{
    public class ShapeCreateModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}

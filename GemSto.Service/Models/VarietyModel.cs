using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class VarietyModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public abstract class ModelBase
    {
        public string CreatedByName { get; set; }
        public string CreatedById { get; set; }
        public string EditedById { get; set; }
    }
}

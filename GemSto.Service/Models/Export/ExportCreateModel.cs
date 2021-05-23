using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Export
{
    public class ExportCreateModel : ModelBase
    {
        public Guid Id { get; set; }
        public int OriginId { get; set; }
        public string Description { get; set; }
        public int ExportNumber { get; set; }
        public DateTimeOffset ExportDate { get; set; }
        public OriginModel Origin { get; set; }
    }
}

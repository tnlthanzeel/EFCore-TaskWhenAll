using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Export
{
    public class GemToExport: ModelBase
    {
        public Guid ExportId { get; set; }
        public int GemId { get; set; }
    }
}

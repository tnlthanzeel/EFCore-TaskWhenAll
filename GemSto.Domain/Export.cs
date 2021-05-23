using GemSto.Common.Enum;
using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GemSto.Domain
{
    public class Export : DomainBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset ExportDate { get; set; }
        public ExportType ExportType { get; set; }
        public ICollection<GemExport> GemExports { get; set; }
        public Origin Origin { get; set; }
        public int OriginId { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public int ExportNumber { get; set; }
        public bool IsExportClosed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Export
{
    public class ExportSummaryModel
    {
        public string Id { get; set; }
        public string CountryName { get; set; }
        public int NumberOfPieces { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public int ExportNumber { get; set; }
        public bool IsExportClosed { get; set; }

    }
}

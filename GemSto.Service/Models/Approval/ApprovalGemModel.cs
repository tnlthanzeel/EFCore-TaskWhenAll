using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class ApprovalGemModel
    {
        public int Id { get; set; }
        public string Variety { get; set; }
        public decimal Weight { get; set; }
        public string Shape { get; set; }
        public decimal Price { get; set; }
        public bool IsTreated { get; set; }
        public string StockNumber { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Description { get; set; }
        public Guid? GemIdentity { get; set; }
        public int? GemId { get; set; }
        public string Seller { get; set; }
        public bool IsDeleted { get; set; }
        public GemStatus GemStatus { get; set; }
        public string Certificate { get; set; }
    }
}

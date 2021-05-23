using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approval
{
    public class ApprovalDetailUpdateModel
    {
        public int Id { get; set; }
        public int GemId { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Description { get; set; }

    }
}

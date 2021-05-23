using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models
{
    public class DashBoardData
    {
        public int InStock { get; set; }
        public int Certification { get; set; }
        public int ThirdPartyCertification { get; set; }
        public int Exported { get; set; }
        public int ThirdPartyExported { get; set; }
        public int Approval { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal TotalStockValue { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal TotalUnpaidAndRemainingPartialPayment { get; set; }
    }
}

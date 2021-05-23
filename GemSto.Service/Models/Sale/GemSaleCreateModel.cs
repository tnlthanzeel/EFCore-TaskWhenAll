using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class GemSaleCreateModel
    {
        public int GemId { get; set; }
        public decimal? Amount { get; set; }
        public bool IsCertificatePending { get; set; }
    }
}

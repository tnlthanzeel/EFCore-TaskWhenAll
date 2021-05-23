using GemSto.Service.Models.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class SellerBulkPaymentCreateModel : SellerPaymentCreateModel
    {
        public bool IsFullPayment { get; set; }
    }
}

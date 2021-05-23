using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Sale
{
    public class BuyerBulkPaymentCreateModel : SalePaymentCreateModel
    {
        public bool IsFullPayment { get; set; }
    }



}

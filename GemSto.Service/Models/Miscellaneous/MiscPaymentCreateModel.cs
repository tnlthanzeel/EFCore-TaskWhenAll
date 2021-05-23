using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Service.Models.Miscellaneous
{
    public class MiscPaymentCreateModel
    {

        public int PrimaryMiscCat { get; set; }
        public int? SubMiscCat { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public string Description { get; set; }
        public int? ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public decimal Amount { get; set; }
    }
}

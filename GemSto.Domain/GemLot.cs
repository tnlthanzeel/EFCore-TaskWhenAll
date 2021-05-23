using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Domain
{
    public class GemLot
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalAmountPaidToSeller { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public List<Gem> Gems { get; set; }
        public PaymentStatus PaymentStatusToSeller { get; set; }
        public decimal BrokerFee { get; set; }
    }
}

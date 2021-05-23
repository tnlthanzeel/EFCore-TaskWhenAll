using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GemSto.Common.Enum
{
    public enum PaymentStatus
    {
        [Description("paid")]
        Paid = 1,
        [Description("unpaid")]
        Unpaid = 2,
        [Description("partial")]
        Partial = 3,
    }
}

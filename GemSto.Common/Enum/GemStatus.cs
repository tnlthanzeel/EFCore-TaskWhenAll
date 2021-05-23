using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GemSto.Common.Enum
{
    public enum GemStatus
    {
        [Description("Deleted")]
        Deleted = -4,
        [Description("Single And Lot ")]
        SingleAndLot = -3,
        [Description("All ")]
        All = -2,
        [Description("Gem Lot")]
        GemLot = -1,
        [Description("sold")]
        Sold = 1,
        [Description("exported")]
        Exported = 2,
        [Description("approval")]
        Approval = 3,
        [Description("certification")]
        Certification = 4,
        [Description("returned")]
        Returned = 5,
        [Description("in stock")]
        InStock = 6,
        [Description("lost")]
        Lost = 7,
        [Description("pending")]
        SoldCP = 8
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GemSto.Common.Enum
{
    public enum Remark
    {
        [Description("Created")]
        Created = 1,
        [Description("Edited")]
        Edited = 2,
        [Description("Deleted")]
        Deleted = 3
    }
}

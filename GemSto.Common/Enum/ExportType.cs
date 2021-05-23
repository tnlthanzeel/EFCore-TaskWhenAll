using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GemSto.Common.Enum
{
    public enum ExportType
    {
        [Description("Own")]
        Own = 1,
        [Description("Third Party")]
        ThirdParty = 2
    }
}

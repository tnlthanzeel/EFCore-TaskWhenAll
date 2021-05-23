using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Miscellaneous
{
    public class MiscellaneousUpdateModel : MiscellaneousModel
    {
        public DateTimeOffset EditedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}

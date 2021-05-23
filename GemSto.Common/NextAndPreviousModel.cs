using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Common
{
    public class NextAndPreviousModel
    {
        public int? PreviousId { get; set; }
        public Guid? PreviousIdentity { get; set; }
        public int? NextId { get; set; }
        public Guid? NextIdentity { get; set; }
    }
}

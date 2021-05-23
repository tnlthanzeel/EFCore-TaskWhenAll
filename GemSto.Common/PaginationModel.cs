using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Common
{
    public class PaginationModel<T> : PaginationBase where T : class
    {
        public int TotalRecords { get; set; }

        public IEnumerable<T> Details { get; set; } = new List<T>();
        public dynamic ExtensionData { get; set; }
    }
}

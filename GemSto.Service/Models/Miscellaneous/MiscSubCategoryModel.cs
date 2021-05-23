using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Miscellaneous
{
    public class MiscSubCategoryModel
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int MiscCategoryId { get; set; }
    }
}

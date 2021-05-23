using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Miscellaneous
{
    public class MiscCategoryModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool HasSubCategory { get; set; }
    }
}

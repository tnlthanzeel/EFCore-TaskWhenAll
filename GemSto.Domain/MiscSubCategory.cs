using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Domain
{
    public class MiscSubCategory : DomainBase
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int MiscCategoryId { get; set; }
        public MiscCategory MiscCategory { get; set; }
    }
}

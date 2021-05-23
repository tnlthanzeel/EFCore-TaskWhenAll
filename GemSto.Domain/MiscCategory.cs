using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Domain
{
    public class MiscCategory : DomainBase
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool HasSubCategory { get; set; }
        public ICollection<MiscSubCategory> MiscSubCategories { get; set; }
    }
}

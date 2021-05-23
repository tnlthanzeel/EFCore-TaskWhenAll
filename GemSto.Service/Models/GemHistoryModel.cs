using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Models
{
    public class GemHistoryModel
    {
        public string Action { get; set; }
        public string Description { get; set; }
        public string CreatedByName { get; set; }
        public string Date { get; set; }
        public string GemStatusString { get; set; }
    }
}

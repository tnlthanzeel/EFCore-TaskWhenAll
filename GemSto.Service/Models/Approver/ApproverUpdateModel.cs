using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models.Approver
{
    public class ApproverUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}

using GemSto.Domain.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GemSto.Domain
{
    public class MiscPayments : DomainBase
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public int PrimaryMiscCat { get; set; }
        public int? SubMiscCat { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public int? ParticipantId { get; set; }

        [MaxLength(250)]
        public string ParticipantName { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey(nameof(PrimaryMiscCat))]
        public MiscCategory MiscCategory { get; set; }


        [ForeignKey(nameof(SubMiscCat))]
        public MiscSubCategory MiscSubCategory { get; set; }

        [ForeignKey(nameof(ParticipantId))]
        public Participant Participant { get; set; }
    }
}

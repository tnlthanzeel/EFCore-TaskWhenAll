using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Service.Models
{
    public class GemLotCreateList
    {
        public int? Id { get; set; }
        public decimal InitialCost { get; set; }
        public decimal InitialWeight { get; set; }
        public bool IsTreated { get; set; }
        public ShapeModel Shape { get; set; }
        public VarietyModel Variety { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public double? BrokerFee { get; set; }
        public string Note { get; set; }
        public Guid Identity { get; set; }

    }
}

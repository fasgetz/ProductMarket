using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class OrderStatus
    {
        public int Id { get; set; }
        public int? IdOrder { get; set; }
        public byte? IdStatus { get; set; }
        public DateTime? Date { get; set; }
        public string Commentary { get; set; }

        public virtual Order IdOrderNavigation { get; set; }
        public virtual TypeStatusOrder IdStatusNavigation { get; set; }
    }
}

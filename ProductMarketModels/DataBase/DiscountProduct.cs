using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class DiscountProduct
    {
        public int Id { get; set; }
        public int? IdProduct { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public double? ProcentDiscount { get; set; }

        public virtual Product IdProductNavigation { get; set; }
    }
}

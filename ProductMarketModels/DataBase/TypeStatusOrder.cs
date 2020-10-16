using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class TypeStatusOrder
    {
        public TypeStatusOrder()
        {
            OrderStatus = new HashSet<OrderStatus>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class Order
    {
        public Order()
        {
            OrderStatus = new HashSet<OrderStatus>();
            ProductsInOrder = new HashSet<ProductsInOrder>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Commentary { get; set; }
        public string Address { get; set; }

        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<ProductsInOrder> ProductsInOrder { get; set; }
    }
}

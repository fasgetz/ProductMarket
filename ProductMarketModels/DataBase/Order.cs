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

        // Дата заказа
        public DateTime DateOrder { get; set; }

        // Общая стоимость заказа
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<ProductsInOrder> ProductsInOrder { get; set; }


        /// <summary>
        /// Айди покупателя
        /// </summary>
        public string PayerID { get; set; }
        /// <summary>
        /// Айди покупателя
        /// </summary>
        public string PaymentId { get; set; }
        /// <summary>
        /// Токен
        /// </summary>
        public string token { get; set; }

    }
}

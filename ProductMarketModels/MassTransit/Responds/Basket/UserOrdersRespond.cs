using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Responds.Basket
{
    /// <summary>
    /// Получить заказы пользователя
    /// </summary>
    public partial class UserOrdersRespond
    {

        /// <summary>
        /// Список заказов пользователя
        /// </summary>
        public List<Order> userOrders { get; set; }
    }
}

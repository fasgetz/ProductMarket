using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Basket
{

    /// <summary>
    /// Запрос на получение заказов пользователя по имени
    /// </summary>
    public partial class UserOrdersRequest
    {

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string user { get; set; }
    }
}

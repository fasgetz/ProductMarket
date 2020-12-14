using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Basket
{
    /// <summary>
    /// Запрос корзины продуктов
    /// </summary>
    public partial class orderBasketRequest
    {
        public OrderBasket basket { get; set; }
    }
}

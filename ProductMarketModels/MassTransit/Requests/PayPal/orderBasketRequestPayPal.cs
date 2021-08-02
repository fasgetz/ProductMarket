using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.PayPal
{
    public partial class orderBasketRequestPayPal
    {
        public OrderBasket basket { get; set; }
    }
}

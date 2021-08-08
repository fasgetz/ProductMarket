using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Fondy
{
    public partial class orderBasketRequestFondy
    {
        public OrderBasket basket { get; set; }
    }
}

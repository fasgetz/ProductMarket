using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Stripe
{

    /// <summary>
    /// 
    /// </summary>
    public partial class orderBasketRequestStripe
    {
        public OrderBasket basket { get; set; }
    }
}

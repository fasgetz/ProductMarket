using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Basket
{
    public partial class getBasketProductsRequest
    {
        public ViewModels.Basket.Basket basket { get; set; }
    }
}

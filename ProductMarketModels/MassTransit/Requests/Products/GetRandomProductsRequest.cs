using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{
    public partial class GetRandomProductsRequest
    {
        public int countProducts { get; set; }
    }
}

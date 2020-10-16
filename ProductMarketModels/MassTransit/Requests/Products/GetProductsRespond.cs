using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{
    public class GetProductsRespond
    {
        public List<Product> Products { get; set; }

        public GetProductsRespond(List<Product> products)
        {
            this.Products = products;
        }
    }
}

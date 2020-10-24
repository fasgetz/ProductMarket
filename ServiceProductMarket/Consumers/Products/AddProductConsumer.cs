using MassTransit;
using ProductMarketModels;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class AddProductConsumer : IConsumer<Product>
    {
        private readonly IProductService service;

        public AddProductConsumer(IProductService service)
        {
            this.service = service;
        }

        public Task Consume(ConsumeContext<Product> context)
        {
            service.AddProduct(context.Message);

            return Task.CompletedTask;
        }
    }
}

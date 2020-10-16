using MassTransit;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class AddProductConsumer : IConsumer<Product>
    {
        public AddProductConsumer()
        {

        }

        public Task Consume(ConsumeContext<Product> context)
        {
            return Task.CompletedTask;
        }
    }
}

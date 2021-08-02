using MassTransit;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class GetRandomProductsConsumer : IConsumer<GetRandomProductsRequest>
    {
        private readonly IProductService service;

        public GetRandomProductsConsumer(IProductService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetRandomProductsRequest> context)
        {

            List<Product> list = new List<Product>();

            if (context.Message.discount == true)
            {
                list = await service.GetRandomDiscountProducts(context.Message.countProducts);
            }
            else
            {
                list = await service.GetRandomProdutsAll(context.Message.countProducts);
            }

            

            await context.RespondAsync<GetProductsRespond>(new GetProductsRespond(list));
        }
    }
}

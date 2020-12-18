using MassTransit;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class GetProductsNameConsumer : IConsumer<GetProductsNameRequest>
    {
        private readonly IProductService service;

        public GetProductsNameConsumer(IProductService service)
        {
            this.service = service;
        }


        public async Task Consume(ConsumeContext<GetProductsNameRequest> context)
        {
            var products = await service.GetProducts(context.Message.name, context.Message.page, context.Message.count, context.Message.discount);


            await context.RespondAsync<GetProductsRespond>(new GetProductsRespond(products));
        }
    }
    
}

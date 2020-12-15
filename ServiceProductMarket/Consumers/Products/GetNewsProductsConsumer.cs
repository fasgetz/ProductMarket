using MassTransit;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class GetNewsProductsConsumer : IConsumer<GetNewsProductsRequest>
    {
        private readonly IProductService service;

        public GetNewsProductsConsumer(IProductService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetNewsProductsRequest> context)
        {
            var list = await service.GetNewsProduct(context.Message.countProducts);            

            await context.RespondAsync<GetProductsRespond>(new GetProductsRespond(list));
       
        }
    }
}

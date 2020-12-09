using MassTransit;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests.Products.Responds;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class ExistProductConsumer : IConsumer<IdProductRequest>
    {
        private readonly IProductService service;


        public ExistProductConsumer(IProductService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<IdProductRequest> context)
        {
            var exist = await service.ExistProduct(context.Message.idProduct);


            await context.RespondAsync<ExistProductRespond>(new ExistProductRespond() { existProduct = exist });
        }
    }
}

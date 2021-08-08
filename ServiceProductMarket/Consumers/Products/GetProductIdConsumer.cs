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
    public class GetProductIdConsumer : IConsumer<GetProductId>
    {
        private readonly IProductService service;

        public GetProductIdConsumer(IProductService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Получить продукт по подкатегории
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetProductId> context)
        {

            var product = await service.getProductId(context.Message.idProduct);


            await context.RespondAsync<Product>(product);
        }
    }
    
    
}

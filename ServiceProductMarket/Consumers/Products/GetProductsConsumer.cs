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
    public class GetProductsConsumer : IConsumer<GetProductsRequest>
    {
        public GetProductsConsumer()
        {

        }

        private readonly IProductService service;

        public GetProductsConsumer(IProductService service)
        {
            this.service = service;
        }


        public async Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            await context.RespondAsync<GetProductsRespond>(new GetProductsRespond(await service.GetProducts(context.Message.IdCategoryProduct)));


            //var list = new List<Product>()
            //{
            //    new Product()
            //    {
            //        Name = "tested"
            //    }
            //};

            //await context.RespondAsync<GetProductsRequest>(context);
            //var result = await service.GetProducts(context.Message.IdCategoryProduct);
            //wait context.RespondAsync<GetProductsRequest>(new GetProductsRespond(null));            
        }
    }
}

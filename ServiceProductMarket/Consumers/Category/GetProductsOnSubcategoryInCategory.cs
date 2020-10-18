using MassTransit;
using ProductMarketModels.MassTransit.Requests.Categories;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Category
{
    public class GetProductsOnSubcategoryInCategoryConsumer : IConsumer<GetProductsRequest>
    {
        private readonly ICategoriesService service;

        public GetProductsOnSubcategoryInCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetProductsRequest> context)
        {
            await context.RespondAsync<GetCategoriesRespond>(new GetCategoriesRespond(await service.GetCategoriesProducts()));

            
        }
    }
}

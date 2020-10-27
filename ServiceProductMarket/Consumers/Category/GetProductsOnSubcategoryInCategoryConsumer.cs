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
    public class GetProductsOnSubcategoryInCategoryConsumer : IConsumer<GetSubcategoriesRequest>
    {
        private readonly ICategoriesService service;

        public GetProductsOnSubcategoryInCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<GetSubcategoriesRequest> context)
        {
            var categories = await service.GetCategoriesProducts();

            await context.RespondAsync<GetCategoriesRespond>(new GetCategoriesRespond(categories));

            
        }
    }
}

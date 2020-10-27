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
    public class GetSubCategoriesOnCategoryConsumer : IConsumer<SubcategoriesRequest>
    {
        private readonly ICategoriesService service;

        public GetSubCategoriesOnCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<SubcategoriesRequest> context)
        {
            var category = await service.GetCategoriesProducts(context.Message.IdCategory);

            await context.RespondAsync<GetSubcategoriesRespond>(new GetSubcategoriesRespond(category));

            
        }
    }
}

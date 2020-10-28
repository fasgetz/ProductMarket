using MassTransit;
using ProductMarketModels;
using ProductMarketServices.Categories;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Category
{
    public class EditCategoryConsumer : IConsumer<CategoryProduct>
    {
        private readonly ICategoriesService service;

        public EditCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public Task Consume(ConsumeContext<CategoryProduct> context)
        {
            service.EditCategory(context.Message);            

            return Task.CompletedTask;
        }
    }
}

using MassTransit;
using ProductMarketServices.Categories;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Category
{
    public class AddCategoryConsumer : IConsumer<ProductMarketModels.MassTransit.Requests.Categories.Models.Category>
    {
        private readonly ICategoriesService service;

        public AddCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public Task Consume(ConsumeContext<ProductMarketModels.MassTransit.Requests.Categories.Models.Category> context)
        {
            service.AddCategory(context.Message);            

            return Task.CompletedTask;
        }
    }
}

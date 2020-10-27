using MassTransit;
using ProductMarketModels;
using ProductMarketServices.Categories;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Category
{
    public class AddSubCategoryConsumer : IConsumer<SubCategoryProduct>
    {
        private readonly ICategoriesService service;

        public AddSubCategoryConsumer(ICategoriesService service)
        {
            this.service = service;
        }

        public Task Consume(ConsumeContext<SubCategoryProduct> context)
        {
            service.AddSubCategory(context.Message);            

            return Task.CompletedTask;
        }
    }
}

using MassTransit;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Categories;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class GetProductsConsumer : IConsumer<GetSubcategoriesRequest>
    {
        public GetProductsConsumer()
        {

        }

        private readonly IProductService service;
        private readonly ICategoriesService categoryService;

        public GetProductsConsumer(IProductService service, ICategoriesService categoryService)
        {
            this.service = service;
            this.categoryService = categoryService;
        }


        /// <summary>
        /// Получить продукт по подкатегории
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<GetSubcategoriesRequest> context)
        {

            var products = await service.GetProducts(context.Message.IdCategoryProduct);
            var subCategory = await categoryService.GetSubCategoryProductData(context.Message.IdCategoryProduct);


            await context.RespondAsync<GetProductsRespond>(new GetProductsRespond(products, subCategory));         
        }
    }
}

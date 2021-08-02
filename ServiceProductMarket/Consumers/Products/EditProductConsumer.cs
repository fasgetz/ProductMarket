using MassTransit;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.ProductsController;
using ProductMarketServices.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Products
{
    public class EditProductConsumer : IConsumer<EditProductViewModel>
    {
        private readonly IProductService service;

        public EditProductConsumer(IProductService service)
        {
            this.service = service;
        }

        public Task Consume(ConsumeContext<EditProductViewModel> context)
        {
            var vm = context.Message;
            Product product = new Product()
            {
                Name = vm.name,
                Id = vm.id,
                Amount = vm.count,
                Price = vm.price,
                description = vm.description,
                IdSubCategory = (short)vm.subcategoryId,
                Poster = vm.image
            };






            service.EditProduct(product);

            return Task.CompletedTask;
        }
    }
}

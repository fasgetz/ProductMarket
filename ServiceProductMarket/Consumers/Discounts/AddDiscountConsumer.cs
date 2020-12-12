using AutoMapper;
using MassTransit;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.DiscountController;
using ProductMarketServices.Products;
using ProductMarketServices.ProductsDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Discounts
{
    public class AddDiscountConsumer : IConsumer<AddDiscountViewModel>
    {
        private readonly IProductDiscountService service;
        private readonly IMapper _mapper;

        public AddDiscountConsumer(IProductDiscountService service, IMapper mapper)
        {
            this.service = service;
            _mapper = mapper;
        }

        public Task Consume(ConsumeContext<AddDiscountViewModel> context)
        {
            var discount = _mapper.Map<DiscountProduct>(context.Message);


            service.AddDiscountProduct(discount);


            return Task.CompletedTask;
        }
    }
}

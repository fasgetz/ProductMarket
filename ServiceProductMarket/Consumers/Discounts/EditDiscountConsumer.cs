using AutoMapper;
using MassTransit;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.DiscountController;
using ProductMarketServices.ProductsDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Discounts
{
    public class EditDiscountConsumer : IConsumer<EditDiscountViewModel>
    {
        private readonly IProductDiscountService service;
        private readonly IMapper _mapper;

        public EditDiscountConsumer(IProductDiscountService service, IMapper mapper)
        {
            this.service = service;
            _mapper = mapper;
        }


        public Task Consume(ConsumeContext<EditDiscountViewModel> context)
        {
            var discount = _mapper.Map<DiscountProduct>(context.Message);


            service.EditDiscountProduct(discount);


            return Task.CompletedTask;
        }
    }
}

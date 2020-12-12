using MassTransit;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests.Discounts.Admin.Requests;
using ProductMarketModels.MassTransit.Requests.Discounts.Admin.Responds;
using ProductMarketServices.ProductsDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Discounts
{
    public class RemoveDiscountConsumer : IConsumer<IdDiscountRequest>
    {

        private readonly IProductDiscountService service;

        public RemoveDiscountConsumer(IProductDiscountService service)
        {
            this.service = service;
        }


        public async Task Consume(ConsumeContext<IdDiscountRequest> context)
        {
            var removed = await service.RemoveDiscountProduct(context.Message.IdDiscount);


            await context.RespondAsync<RemovedDiscountRespond>(new RemovedDiscountRespond() { HasRemoved = removed });
        }
    }
}

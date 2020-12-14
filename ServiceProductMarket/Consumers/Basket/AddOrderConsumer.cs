using MassTransit;
using ProductMarketModels.MassTransit.Requests.Basket;
using ProductMarketModels.MassTransit.Responds.Basket;
using ProductMarketServices.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Basket
{
    public class AddOrderConsumer : IConsumer<orderBasketRequest>
    {
        private readonly IBasketService service;

        public AddOrderConsumer(IBasketService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<orderBasketRequest> context)
        {
            var order = await service.UserPay(context.Message.basket);

            await context.RespondAsync<getOrderRespond>(new getOrderRespond() { order = order });
        }
    }
}

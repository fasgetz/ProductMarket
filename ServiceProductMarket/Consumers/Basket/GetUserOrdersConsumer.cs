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
    public class GetUserOrdersConsumer : IConsumer<UserOrdersRequest>
    {

        private readonly IBasketService service;

        public GetUserOrdersConsumer(IBasketService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<UserOrdersRequest> context)
        {
            var orders = await service.GetUserOrders(context.Message.user);

            await context.RespondAsync<UserOrdersRespond>(new UserOrdersRespond() { userOrders = orders });
        }


    }
}

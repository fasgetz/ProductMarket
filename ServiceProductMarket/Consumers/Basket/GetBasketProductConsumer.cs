using MassTransit;
using ProductMarketModels.MassTransit.Requests.Basket;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketModels.MassTransit.Responds.Basket;
using ProductMarketServices.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Basket
{
    public class GetBasketProductConsumer : IConsumer<getBasketProductsRequest>
    {
        private readonly IBasketService service;

        public GetBasketProductConsumer(IBasketService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<getBasketProductsRequest> context)
        {
            var basket = await service.GetProductsFromBasket(context.Message.basket);

            await context.RespondAsync<getBasketProductsRespond>(new getBasketProductsRespond() { basket = basket });


        }
    }
}

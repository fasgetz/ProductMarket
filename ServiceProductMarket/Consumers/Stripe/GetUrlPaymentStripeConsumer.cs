using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using ProductMarketModels.MassTransit.Requests.Stripe;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketServices.Basket;
using ProductMarketServices.Stripe;
using ServiceProductMarket.Models.Fondy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Stripe
{
    public class GetUrlPaymentStripeConsumer : IConsumer<orderBasketRequestStripe>
    {
        private readonly IStripeService service;
        private readonly IBasketService basketService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private readonly IMemoryCache cache;

        public GetUrlPaymentStripeConsumer(IMemoryCache cache, IStripeService service, IBasketService basketService, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.service = service;
            this.basketService = basketService;
            this.config = config;
            this.cache = cache;
        }

        public async Task Consume(ConsumeContext<orderBasketRequestStripe> context)
        {
            //var orders = await service.GetUserOrders(context.Message.user);
            var data = await basketService.GetBasketProducts(context.Message.basket);

            // Высчитываем скидки
            foreach (var item in data.Where(i => i.ProcentDiscount != null))
            {
                item.Price -= item.Price.Value / 100 * Convert.ToDecimal(item.ProcentDiscount.Value);
            }

            // Получаем ссылку платежа
            var result = service.createPayment(data);


            /// Модель для кэша
            BasketFondyModel model = new BasketFondyModel()
            {
                userName = context.Message.basket.userName,
                products = data,
                commentary = context.Message.basket.commentary
            };

            // Необходимо запомнить в кэш номер платежа и продукты, чтобы в дальнейшем подтвердить
            cache.Set(result.sessionId, model, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });


            // Возвращаем ссылку на платежку Fondy
            await context.RespondAsync<GetUrlPayment>(new GetUrlPayment() { url = result.url });
        }
    }
}

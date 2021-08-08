using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProductMarketModels.MassTransit.Requests.Fondy;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketModels.PayPal;
using ProductMarketServices.Basket;
using ProductMarketServices.Fondy;
using ProductMarketServices.PayPal;
using ServiceProductMarket.Models.Fondy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Fondy
{
    public class GetUrlPaymentFondyConsumer : IConsumer<orderBasketRequestFondy>
    {
        private readonly IFondyService service;
        private readonly IBasketService basketService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private readonly IMemoryCache cache;

        public GetUrlPaymentFondyConsumer(IMemoryCache cache, IFondyService service, IBasketService basketService, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.service = service;
            this.basketService = basketService;
            this.config = config;
            this.cache = cache;
        }

        public async Task Consume(ConsumeContext<orderBasketRequestFondy> context)
        {
            //var orders = await service.GetUserOrders(context.Message.user);
            var data = await basketService.GetBasketProducts(context.Message.basket);

            // Высчитываем скидки
            foreach (var item in data.Where(i => i.ProcentDiscount != null))
            {
                item.Price -= item.Price.Value / 100 * Convert.ToDecimal(item.ProcentDiscount.Value);
            }

            var totalSum = data.Sum(i => i.Price.Value);


            var val = Convert.ToInt32(totalSum.ToString("0.00").Replace(",", "").Replace(".", ""));

            // Получаем ссылку платежа
            var result = service.createPayment(val);


            /// Модель для кэша
            BasketFondyModel model = new BasketFondyModel()
            {
                userName = context.Message.basket.userName,
                products = data,
                commentary = context.Message.basket.commentary
            };

            // Необходимо запомнить в кэш номер платежа и продукты, чтобы в дальнейшем подтвердить
            cache.Set(result.paymentId, model, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });


            // Возвращаем ссылку на платежку Fondy
            await context.RespondAsync<GetUrlPayment>(new GetUrlPayment() { url = result.url });
        }
    }
}

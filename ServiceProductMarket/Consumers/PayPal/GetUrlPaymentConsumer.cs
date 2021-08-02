using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProductMarketModels.MassTransit.Requests.Basket;
using ProductMarketModels.MassTransit.Requests.PayPal;
using ProductMarketModels.MassTransit.Responds.Basket;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketModels.PayPal;
using ProductMarketServices.Basket;
using ProductMarketServices.PayPal;
using ServiceProductMarket.Models.PayPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.PayPal
{
    public class GetUrlPaymentConsumer : IConsumer<orderBasketRequestPayPal>
    {

        private readonly IPayPalService service;
        private readonly IBasketService basketService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration config;
        private readonly IMemoryCache cache;

        public GetUrlPaymentConsumer(IMemoryCache cache, IPayPalService service, IBasketService basketService, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.service = service;
            this.basketService = basketService;
            this.config = config;
            this.cache = cache;
        }

        public async Task Consume(ConsumeContext<orderBasketRequestPayPal> context)
        {
            //var orders = await service.GetUserOrders(context.Message.user);
            var data = await basketService.GetBasketProducts(context.Message.basket);

            // Высчитываем скидки
            foreach (var item in data.Where(i => i.ProcentDiscount != null))
            {
                item.Price -= item.Price.Value / 100 * Convert.ToDecimal(item.ProcentDiscount.Value);
            }

            IEnumerable<ProductModelPayPal> items = data.Select(s => new ProductModelPayPal()
            {
                count = s.count,
                description = s.description,
                name = s.Name,
                price = s.Price.Value,
                sku = s.id.ToString(),
                currency = config.GetValue<string>("PayPalConfig:currency")
            });

            var payment = service.createPayment(items);


            var getPayment = service.GetPayment(payment.id);


            /// Модель для кэша
            BasketPayPalModel model = new BasketPayPalModel()
            {
                userName = context.Message.basket.userName,
                transactions = getPayment.transactions,
                commentary = context.Message.basket.commentary
            };
            
            // Необходимо запомнить в кэш транзакцию, чтобы ее в дальнейшем подтвердить
            cache.Set(payment.id, model, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });


            var url = getPayment.links.FirstOrDefault(i => i.method == "REDIRECT").href;

            await context.RespondAsync<GetUrlPayment>(new GetUrlPayment() { url = url });
        }


    }
}

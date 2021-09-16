using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProductMarketModels.MassTransit.Requests.Stripe;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketServices.Basket;
using ProductMarketServices.EmailServiceNew;
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
        private readonly IEmalService emailService;
        private readonly IMemoryCache cache;

        public GetUrlPaymentStripeConsumer(ProductMarketServices.EmailServiceNew.IEmalService emailService, IMemoryCache cache, IStripeService service, IBasketService basketService, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            this.service = service;
            this.basketService = basketService;
            this.config = config;
            this.emailService = emailService;
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


            // Если есть результат, то сделать рассылку
            if (result != null)
            {
                try
                {

                    var sum = data.Sum(i => i.Price * i.count).Value.ToString("N2");

                    // Список адресов для рассылки
                    var addresses = config.GetValue<string>("emailsAddresses:emails").Replace(" ", "").Split(',');

                    // Отправить адресатам, которые в рассылке
                    foreach (var item in addresses)
                    {
                        await emailService.SendEmailAsync(item, $"Создание платежа для {context.Message.basket.userName} на сумму {sum} EUR", $"<b>{DateTime.Now}</b> Платеж <b>{result.paymentIntentId}</b> успешно создан и ждет оплаты от участника системы {context.Message.basket.userName} <br>Сумма к оплате: {sum} EUR");
                    }

                    // Отправить самому покупателю
                    await emailService.SendEmailAsync(context.Message.basket.userName, $"Создание платежа на сумму {sum} EUR", $"<b>{DateTime.Now}</b> Платеж <b>{result.paymentIntentId}</b> успешно создан и ждет оплаты от участника системы {context.Message.basket.userName} <br>Сумма к оплате: {sum} EUR");

                }
                catch (Exception ex)
                {

                }
            }


            /// Модель для кэша
            BasketFondyModel model = new BasketFondyModel()
            {
                userName = context.Message.basket.userName,
                products = data,
                commentary = context.Message.basket.commentary
            };

            // Необходимо запомнить в кэш номер платежа и продукты, чтобы в дальнейшем подтвердить
            cache.Set(result.paymentIntentId, model, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });


            // Возвращаем ссылку на платежку Fondy
            await context.RespondAsync<GetUrlPayment>(new GetUrlPayment() { url = result.url });
        }
    }
}

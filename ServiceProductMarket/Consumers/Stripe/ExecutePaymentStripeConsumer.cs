using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Stripe;
using ProductMarketModels.MassTransit.Responds.Stripe;
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
    public class ExecutePaymentStripeConsumer : IConsumer<ExecutePaymentStripeRequest>
    {
        private readonly IStripeService StripeService;
        private readonly IConfiguration config;
        private readonly IEmalService emailService;
        private readonly IMemoryCache cache;
        private readonly IBasketService basketService;

        public ExecutePaymentStripeConsumer(IConfiguration config, ProductMarketServices.EmailServiceNew.IEmalService emailService, IMemoryCache cache, IStripeService StripeService, IBasketService basketService)
        {
            this.config = config;
            this.emailService = emailService;
            this.cache = cache;
            this.StripeService = StripeService;
            this.basketService = basketService;
        }


        public async Task Consume(ConsumeContext<ExecutePaymentStripeRequest> context)
        {
            BasketFondyModel model = cache.Get<BasketFondyModel>(context.Message.session_id);

            var payment = StripeService.SuccessPayment(context.Message.session_id);

            
            // Если платеж успешный, то записать в БД
            if (payment.succes == true)
            {
                try
                {
                    var totalPrice = model.products.Sum(i => i.Price * i.count);


                    // Формируем заказ
                    ProductMarketModels.Order order = new ProductMarketModels.Order()
                    {
                        PayerID = model.userName,
                        PaymentId = payment.idPayment,
                        token = payment.session_id,
                        // Сумма заказа
                        TotalPrice = totalPrice.Value,
                        UserId = model.userName,
                        //Address = orderBasket.address,
                        Commentary = model.commentary,
                        DateOrder = DateTime.Now.AddHours(3),
                        OrderStatus = new List<OrderStatus>() // Статус заказа
                        {
                            new OrderStatus()
                            {
                                IdStatus = 1,
                                Date = DateTime.Now
                            }
                        },
                        ProductsInOrder = model.products.Select(i => new ProductsInOrder() { IdProduct = Convert.ToInt32(i.id), Count = (short)Convert.ToInt16(i.count), priceBuy = Convert.ToDecimal(i.Price) }).ToList()
                    };

                    var addedOrder = await basketService.AddOrder(order);

                    // Нужно оповестить всех пользователей об успешной оплате заказа по емейлу
                    // Список адресов для рассылки
                    var addresses = config.GetValue<string>("emailsAddresses:emails").Replace(" ", "").Split(',');


                    // Сумма к выплате с учетом процентов
                    var procentPrice = (double)totalPrice.Value - (((double)totalPrice.Value / 100 * 2.9) + 0.3);

                    // Отправить адресатам, которые в рассылке
                    foreach (var item in addresses)
                    {
                        await emailService.SendEmailAsync(item, $"Платеж на сумму {totalPrice} EUR успешно оплачен", $"<b>{DateTime.Now}</b> Платеж <b>{payment.idPayment}</b> успешно оплачен! <br>Оплаченная сумма: {totalPrice} EUR <br><br>Сумма к зачислению на расчетный счет с учетом комиссии stripe (2.9% + 0.3 евро): <b>{procentPrice} EUR</b>");                        
                    }

                    // Отправить самому покупателю
                    await emailService.SendEmailAsync(model.userName, $"Платеж на сумму {totalPrice} EUR успешно оплачен", $"<b>{DateTime.Now}</b> Платеж <b>{payment.idPayment}</b> успешно оплачен! <br>Оплаченная сумма: {totalPrice} EUR");
                }
                catch (Exception ex)
                {
                    //var message = "abc";
                }
            }


            await context.RespondAsync<ExecutePaymentStripeRespond>(payment);
        }
    }
}

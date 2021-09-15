using Microsoft.Extensions.Configuration;
using ProductMarketModels.MassTransit.Responds.Stripe;
using ProductMarketModels.ViewModels.Basket;
using ProductMarketModels.ViewModels.Stripe;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductMarketServices.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration configSettings;



        public StripeService(IConfiguration configSettings)
        {
            this.configSettings = configSettings;

            /// <summary>
            /// Конфигурация Stripe
            /// </summary>
            StripeConfiguration.ApiKey = configSettings.GetValue<string>("StripeConfig:SecretKey");
        }


        /// <summary>
        /// Проверка успешности платежа
        /// </summary>
        /// <param name="session_id"></param>
        /// <returns></returns>
        public ExecutePaymentStripeRespond SuccessPayment(string session_id)
        {

            PaymentIntentService paymentServiceIntent = new PaymentIntentService();

            var payment = paymentServiceIntent.Get(session_id);

            //var sessionService = new SessionService();
            //Session service = sessionService.Get(session_id);

            if (payment != null)
            {
                ExecutePaymentStripeRespond respond = new ExecutePaymentStripeRespond()
                {
                    session_id = session_id,
                    idPayment = payment.Id,
                    succes = payment.Status == "succeeded" ? true : false
                };


                return respond;
            }


            return new ExecutePaymentStripeRespond();

        }

        /// <summary>
        /// Сгенерировать ссылку на оплату
        /// </summary>
        /// <param name="products">Продукты к оплате</param>
        /// <returns></returns>
        /// <returns></returns>
        public StripeResultModel createPayment(IEnumerable<ProductBasket> products)
        {
            List<SessionLineItemOptions> items = products.Select(i => new SessionLineItemOptions()
            {
                Quantity = i.count,
                Description = i.description,
                Name = i.Name,
                Currency = configSettings.GetValue<string>("StripeConfig:currency"),
                Amount = Convert.ToInt32(i.Price.Value.ToString("0.00").Replace(",", "").Replace(".", ""))
            }).ToList();


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                        "card"
                    },
                     LineItems = items,
                Mode = "payment",
                SuccessUrl = configSettings.GetValue<string>("StripeConfig:successUrl") + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = configSettings.GetValue<string>("StripeConfig:cancelUrl")
            };

            var service = new SessionService();
            Session session = service.Create(options);


            StripeResultModel model = new StripeResultModel()
            {
                sessionId = session.Id,
                url = session.Url,
                paymentIntentId = session.PaymentIntentId
            };

            // null - если ошибка генерации ссылки
            return model;
        }
    }
}

using CloudIpspSDK;
using CloudIpspSDK.Checkout;
using Microsoft.Extensions.Configuration;
using ProductMarketModels.ViewModels.Fondy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketServices.Fondy
{
    /// <summary>
    /// Реализация интерфейса для работы с Fondy
    /// </summary>
    public class FondyService : IFondyService
    {
        private readonly IConfiguration configSettings;

        public FondyService(IConfiguration configSettings)
        {
            this.configSettings = configSettings;


            


        }


        /// <summary>
        /// Сгенерировать ссылку на оплату
        /// </summary>
        /// <param name="amount">Сумма в оплате (странно передается, но например 100.50$ надо записать как 10050</param>
        /// <returns></returns>
        public FondyResultModel createPayment(int amount)
        {
            Config.MerchantId = configSettings.GetValue<int>("FondyConfig:merchantId");
            Config.SecretKey = configSettings.GetValue<string>("FondyConfig:SecretKey");

            // Формируем запрос
            var req = new CheckoutRequest()
            {
                order_id = Guid.NewGuid().ToString("N"),
                order_desc = "Оплата с магазина busmansoft.com",
                currency = configSettings.GetValue<string>("FondyConfig:currency"),
                amount = amount
            };

            var resp = new Url().Post(req);

            // Если ошибки нет и ссылка сгенерировалась, то вернуть ее
            if (resp.Error == null)
            {
                string url = resp.checkout_url;

                // Формируем результат
                FondyResultModel model = new FondyResultModel()
                {
                    url = url,
                    paymentId = resp.payment_id
                };

                return model;
            }

            // null - если ошибка генерации ссылки
            return null;
        }
    }
}

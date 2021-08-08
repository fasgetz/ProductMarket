using ProductMarketModels.MassTransit.Responds.Stripe;
using ProductMarketModels.ViewModels.Basket;
using ProductMarketModels.ViewModels.Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketServices.Stripe
{

    /// <summary>
    /// Интерфейс для работы с платежной системой Stripe
    /// </summary>
    public interface IStripeService
    {


        /// <summary>
        /// Проверка успешности платежа
        /// </summary>
        /// <param name="session_id"></param>
        /// <returns></returns>
        public ExecutePaymentStripeRespond SuccessPayment(string session_id);


        /// Сгенерировать ссылку на оплату
        /// </summary>
        /// <param name="products">Продукты к оплате</param>
        /// <returns></returns>
        public StripeResultModel createPayment(IEnumerable<ProductBasket> products);
    }
}

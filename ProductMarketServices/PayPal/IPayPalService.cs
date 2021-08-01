using PayPal.Api;
using ProductMarketModels.PayPal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketServices.PayPal
{
    public interface IPayPalService
    {
        /// <summary>
        /// Подтверждение платежа
        /// </summary>
        /// <param name="paymentId">Айди платежа</param>
        /// <param name="payerId">Айди плательщика</param>
        /// <param name="transactions">Транзакция</param>
        /// <returns></returns>
        public bool ExecutePayment(string paymentId, string payerId, List<Transaction> transactions);

        /// <summary>
        /// Создание платежа
        /// </summary>
        /// <param name="products">Список продуктов</param>
        /// <returns></returns>
        public Payment createPayment(IEnumerable<ProductModelPayPal> products);

        /// <summary>
        /// Получение данных платежа по айди
        /// </summary>
        /// <returns></returns>
        public Payment GetPayment(string id);
    }
}

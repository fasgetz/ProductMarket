using ProductMarketModels.ViewModels.Fondy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketServices.Fondy
{
    /// <summary>
    /// Интерфейс для работы с системой платежек Fondy
    /// </summary>
    public interface IFondyService
    {
        /// <summary>
        /// Сгенерировать ссылку на оплату
        /// </summary>
        /// <param name="amount">Сумма в оплате (странно передается, но например 100.50$ надо записать как 10050</param>
        /// <returns></returns>
        public FondyResultModel createPayment(int amount);
    }
}

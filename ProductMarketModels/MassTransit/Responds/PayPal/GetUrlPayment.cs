using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Responds.PayPal
{
    /// <summary>
    /// Получить ссылку url на оплату PayPal
    /// </summary>
    public partial class GetUrlPayment
    {
        /// <summary>
        /// url адрес оплаты PayPal
        /// </summary>
        public string url { get; set; }
    }
}

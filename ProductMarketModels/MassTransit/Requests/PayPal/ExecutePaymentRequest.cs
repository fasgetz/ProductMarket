using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.PayPal
{
    /// <summary>
    /// Модель для подтверждения платежа PayPal
    /// </summary>
    public partial class ExecutePaymentRequest
    {
        /// <summary>
        /// Айди плательщика
        /// </summary>
        public string PayerID { get; set; }

        /// <summary>
        /// Айди платежа
        /// </summary>
        public string PaymentID { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
    }
}

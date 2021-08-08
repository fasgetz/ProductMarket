using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Responds.Stripe
{
    public partial class ExecutePaymentStripeRespond
    {
        /// <summary>
        /// Успех платежа
        /// </summary>
        public bool succes { get; set; }

        /// <summary>
        /// Айди платежа
        /// </summary>
        public string idPayment { get; set; }

        /// <summary>
        /// Айди сессии
        /// </summary>
        public string session_id { get; set; }
    }
}

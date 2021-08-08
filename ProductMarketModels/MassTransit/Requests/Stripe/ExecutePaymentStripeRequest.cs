using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Stripe
{
    public partial class ExecutePaymentStripeRequest
    {
        /// <summary>
        /// Айди сессии
        /// </summary>
        public string session_id { get; set; }
    }
}

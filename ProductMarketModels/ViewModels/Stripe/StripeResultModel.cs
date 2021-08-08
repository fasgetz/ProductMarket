using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.ViewModels.Stripe
{
    /// <summary>
    /// Результат возвращения Stripe после создания платежа
    /// </summary>
    public partial class StripeResultModel
    {
        /// <summary>
        /// url платежа
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Айди сессии
        /// </summary>
        public string sessionId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Responds.PayPal
{

    /// <summary>
    /// Модель для подтверждения платежа PayPal
    /// </summary>
    public partial class ExecutePaymentRespond
    {
        /// <summary>
        /// Успех платежа
        /// </summary>
        public bool successPayment { get; set; }
    }
}

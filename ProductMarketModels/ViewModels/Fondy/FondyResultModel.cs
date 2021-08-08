using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.ViewModels.Fondy
{
    /// <summary>
    /// Результат возврата генерации платежа Fondy
    /// </summary>
    public partial class FondyResultModel
    {
        /// <summary>
        /// url платежа
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Айди платежа
        /// </summary>
        public int paymentId { get; set; }
    }
}

using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Models.PayPal
{
    /// <summary>
    /// Корзина товаров для PayPal
    /// </summary>
    public partial class BasketPayPalModel
    {
        public List<Transaction> transactions { get; set; }
        public string userName { get; set; }

        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string commentary { get; set; }
    }
}

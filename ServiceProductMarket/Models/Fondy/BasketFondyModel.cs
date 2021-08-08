using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Models.Fondy
{

    /// <summary>
    /// Корзина товаров для Fondy
    /// </summary>
    public class BasketFondyModel
    {
        public IEnumerable<ProductBasket> products { get; set; }
        public string userName { get; set; }

        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string commentary { get; set; }
    }
}

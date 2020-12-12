using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.ViewModels.Basket
{

    /// <summary>
    /// Сущность продукта корзины
    /// </summary>
    public partial class ProductBasket
    {
        /// <summary>
        /// Номер продукта в БД
        /// </summary>
        public int id { get; set; }

        // Количество
        public int count { get; set; }

        // Сам продукт
        public Product product { get; set; }
    }
}

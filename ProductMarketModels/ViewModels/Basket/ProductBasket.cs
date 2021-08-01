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

        /// <summary>
        /// Количество товара заказано
        /// </summary>
        public int count { get; set; }


        // Данные

        public string Name { get; set; }
        public decimal? Price { get; set; }

        /// <summary>
        /// Всего на складе
        /// </summary>
        public int? Amount { get; set; }
        public byte[] Poster { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string description { get; set; }


        // Процент скидки
        public double? ProcentDiscount { get; set; }


    }
}

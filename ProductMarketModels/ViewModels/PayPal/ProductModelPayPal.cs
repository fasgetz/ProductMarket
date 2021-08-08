using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.PayPal
{

    /// <summary>
    /// Продукт корзины
    /// </summary>
    public class ProductModelPayPal
    {
        /// <summary>
        /// Название
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Цена за 1 шт
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Тэг
        /// </summary>
        public string sku { get; set; }

        /// <summary>
        /// Валюта
        /// </summary>
        public string currency { get; set; }
    }
}

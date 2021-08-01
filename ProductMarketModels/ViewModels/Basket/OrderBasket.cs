using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMarketModels.ViewModels.Basket
{

    /// <summary>
    /// Сущность для формирования оплаты
    /// </summary>
    public partial class OrderBasket
    {

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// Адрес доставки
        /// </summary>
        //[Required]
        //[MinLength(10)]
        public string address { get; set; }

        /// <summary>
        /// Комментарий к доставке
        /// </summary>
        public string commentary { get; set; }

        /// <summary>
        /// Корзина товарова
        /// </summary>
        public Basket basket { get; set; }

    }
}

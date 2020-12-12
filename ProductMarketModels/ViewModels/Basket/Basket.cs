﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.ViewModels.Basket
{
    /// <summary>
    /// Сущность корзины покупок
    /// </summary>
    public partial class Basket
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// Корзина покупок
        /// </summary>
        public List<ProductBasket> products { get; set; } = new List<ProductBasket>();
    }
}

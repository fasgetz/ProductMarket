﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{

    /// <summary>
    /// Реквест для поиска продукта по названию
    /// </summary>
    public partial class GetProductsNameRequest
    {
        public string name { get; set; }
        public int page { get; set; }
        public int count { get; set; }
        public bool discount { get; set; }
    }
}

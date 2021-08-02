using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{
    public partial class GetRandomProductsRequest
    {
        public int countProducts { get; set; }

        /// <summary>
        /// Выборка рандомных товаров, только тех у которых есть скидка
        /// </summary>
        public bool discount { get; set; }
    }
}

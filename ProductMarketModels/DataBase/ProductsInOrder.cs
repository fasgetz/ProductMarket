using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class ProductsInOrder
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? IdProduct { get; set; }
        public short? Count { get; set; }

        /// <summary>
        /// Стоимость по которой куплен товар на момент оплаты
        /// </summary>
        public decimal? priceBuy { get; set; }

        public virtual Product IdProductNavigation { get; set; }
        public virtual Order Order { get; set; }
    }
}

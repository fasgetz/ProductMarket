using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ProductsDiscount
{
    public interface IProductDiscountService
    {
        /// <summary>
        /// Получает все скидки продукта
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>Все скидки продукта</returns>
        public Task<List<DiscountProduct>> GetDiscountsProduct(int idProduct);

        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="discount"></param>
        public void AddDiscountProduct(DiscountProduct discount);
    }
}

using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ProductsDiscount
{
    public class ProductDiscountService : IProductDiscountService
    {
        private ProductMarketContext context;

        public ProductDiscountService(ProductMarketContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Получает все скидки продукта
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>Все скидки продукта</returns>
        public async Task<List<DiscountProduct>> GetDiscountsProduct(int idProduct)
        {
            var discountsProduct = await context.DiscountProduct.Where(i => i.IdProduct == idProduct).ToListAsync();


            return discountsProduct;
        }

        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="discount"></param>
        public async void AddDiscountProduct(DiscountProduct discount)
        {
            using (context = new ProductMarketContext())
            {
                context.DiscountProduct.Add(discount);
                await context.SaveChangesAsync();
            }
        }
    }
}

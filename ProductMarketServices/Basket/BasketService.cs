using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Basket
{
    public class BasketService : IBasketService
    {
        private readonly ProductMarketContext context;

        public BasketService(ProductMarketContext context)
        {
            this.context = context;
        }



        /// <summary>
        /// Получить продукты, которые есть в корзине с подруженной информацией
        /// </summary>
        /// <param name="basket">Корзина</param>
        /// <returns>Корзину продуктов с подгруженной информацией</returns>
        public async Task<ProductMarketModels.ViewModels.Basket.Basket> GetProductsFromBasket(ProductMarketModels.ViewModels.Basket.Basket basket)
        {
            // Массив айдишников добавленных предметов в корзину
            var idsItems = basket.products.Select(i => i.id).ToArray();

            
            // Запрос на выборку продуктов, которые пользователь добавил в корзину
            var products = await context.Product
                .Where(i => idsItems.Contains(i.Id))
                .Select(s => new ProductBasket()
                {
                    id = s.Id,
                    Amount = s.Amount,
                    Name = s.Name,
                    Poster = s.Poster,
                    //count = basket.products.FirstOrDefault().count,
                    Price = s.Price,

                    // Скидка товара
                    ProcentDiscount = s.DiscountProduct.Where(f => f.DateEnd > DateTime.Now && f.DateStart < DateTime.Now).FirstOrDefault().ProcentDiscount
                })
                .ToListAsync();

            products.ForEach(i =>
            {
                i.count = basket.products.FirstOrDefault(s => s.id == i.id).count;
            });

            // Добавляем к корзине товаров 
            basket.products = products;

            // Возвращаем корзину
            return basket;
        }

    }
}

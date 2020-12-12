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
                .Select(s => new Product()
                {
                    Id = s.Id,
                    Amount = s.Amount,
                    Name = s.Name,
                    Poster = s.Poster,
                    Price = s.Price,
                    DiscountProduct = s.DiscountProduct.Where(f => f.DateEnd > DateTime.Now && f.DateStart < DateTime.Now).ToList()
                })
                .ToListAsync();

            // Добавляем к корзине товаров 
            basket.products.ForEach(i =>
                i.product = products.FirstOrDefault(s => s.Id == i.id)
            );

            // Возвращаем корзину
            return basket;
        }

    }
}

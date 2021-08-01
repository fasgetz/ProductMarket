using ProductMarketModels;
using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Basket
{
    public interface IBasketService
    {


        /// <summary>
        /// Выборка продуктов из БД, которые юзер добавил в корзину
        /// </summary>
        /// <param name="orderBasket"></param>
        /// <returns></returns>
        public Task<IEnumerable<ProductBasket>> GetBasketProducts(OrderBasket orderBasket);


        /// <summary>
        /// Добавление заказа в базу данных
        /// </summary>
        /// <param name="orderBasket">Корзина товаров</param>
        /// <returns>Заказ</returns>
        public Task<Order> UserPay(OrderBasket orderBasket);



        /// <summary>
        /// Получить продукты, которые есть в корзине с подруженной информацией
        /// </summary>
        /// <param name="basket">Корзина</param>
        /// <returns>Корзину продуктов с подгруженной информацией</returns>
        public Task<ProductMarketModels.ViewModels.Basket.Basket> GetProductsFromBasket(ProductMarketModels.ViewModels.Basket.Basket basket);

        /// <summary>
        /// Получить список заказов пользователя
        /// </summary>
        /// <param name="UserName">Пользователь</param>
        /// <returns>Список заказов пользователя</returns>
        public Task<List<Order>> GetUserOrders(string UserName);
    }
}

using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Basket;
using ProductMarketServices.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        /// Получить список заказов пользователя
        /// </summary>
        /// <param name="UserName">Пользователь</param>
        /// <returns>Список заказов пользователя</returns>
        public async Task<List<Order>> GetUserOrders(string UserName)
        {
            var orders = await context.Order.Where(i => i.UserId == UserName).ToListAsync();

            return orders;
        }

        /// <summary>
        /// Добавление заказа в базу данных
        /// </summary>
        /// <param name="orderBasket">Корзина товаров</param>
        /// <returns>Заказ</returns>
        public async Task<Order> UserPay(OrderBasket orderBasket)
        {
            // Транзакция на добавление заказа в базу
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {                    
                    // Выборка продуктов из БД, которые юзер добавил в корзину
                    var getProducts = await context.Product
                        .Select(i => new Product()
                        {
                            Id = i.Id,
                            Price = i.Price,
                            Amount = i.Amount,
                            DiscountProduct = i.DiscountProduct.Where(f => f.DateEnd > DateTime.Now && f.DateStart < DateTime.Now).ToList()
                        })
                        .Where(i => orderBasket.basket.products.Select(i => i.id).Contains(i.Id)).ToListAsync(); // выборка продуктов добавленных в корзину


                    // Формируем корзину
                    var products = getProducts
                        //.Where(i => orderBasket.basket.products.Select(i => i.id).Contains(i.Id)) // выборка продуктов добавленных в корзину
                        .Select(s => new ProductBasket()
                        {
                            id = s.Id,
                            Price = s.Price,
                            //Amount = s.Amount,
                            count = orderBasket.basket.products.FirstOrDefault(i => i.id == s.Id).count,
                            // Скидка товара
                            ProcentDiscount = s.DiscountProduct.FirstOrDefault()?.ProcentDiscount
                        }).ToList();




                    // Формируем заказ
                    Order order = new Order()
                    {
                        // Сумма заказа
                        TotalPrice = products.Sum(i => i.ProcentDiscount == null ? i.Price * i.count : (i.Price - (i.Price / 100 * (decimal)i.ProcentDiscount)) * i.count).Value,
                        UserId = orderBasket.userName,
                        Address = orderBasket.address,
                        Commentary = orderBasket.commentary,
                        DateOrder = DateTime.Now,
                        OrderStatus = new List<OrderStatus>() // Статус заказа
                        {
                            new OrderStatus()
                            {
                                IdStatus = 1,
                                Date = DateTime.Now
                            }
                        },
                        ProductsInOrder = orderBasket.basket.products.Select(i => new ProductsInOrder() { IdProduct = i.id, Count = (short)i.count }).ToList()
                    };                   

                    // Добавляем в базу данных
                    context.Order.Add(order);

                    // Теперь необходимо вычесть разницу между имеющимся на складе товаром и купленным
                    foreach (var item in getProducts)
                    {
                        item.Amount = item.Amount - products.FirstOrDefault(s => s.id == item.Id).count;

                        // Указать, что запись изменилась
                        context.Product.Attach(item);
                        context.Entry(item)
                            .Property(c => c.Amount).IsModified = true;
                    }

                    context.SaveChanges();

                    // Получаем номер заказа в базе данных
                    order = context.Order.Select(i => new Order()
                    {
                        Id = i.Id,
                        DateOrder = i.DateOrder,
                        Address = i.Address,
                        Commentary = i.Commentary,
                        TotalPrice = i.TotalPrice,
                        UserId = i.UserId
                    })
                    .OrderByDescending(i => i.Id).Take(1).FirstOrDefault();


                    // Завершаем транзакцию
                    transaction.Commit();


                    // В отдельном потоке оповестить пользователя по почте о формировании заказа
                    Thread thread = new Thread(new ThreadStart(() => {
                        INotificationService notificationService = new NotificationServiceEmail();

                        notificationService.NotificationUser(order);


                    }));

                    thread.Start();


                    return order;
                }
                catch (Exception)
                {
                    // Откатываем транзакцию
                    transaction.Rollback();

                    return null;
                }
            }
            
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

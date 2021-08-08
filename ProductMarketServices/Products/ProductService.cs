using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductMarketModels;
using ProductMarketModels.Constants;
using ProductMarketServices.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductMarketServices.Products
{

    public static class RandFisher
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class ProductService : IProductService
    {
        private ProductMarketContext context;

        public ProductService(ProductMarketContext context)
        {
            this.context = context;
        }

        #region Запросы к сервису эластика

        /// <summary>
        /// Добавление продукта в документы эластика
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        private async Task AddToElasticProduct(Product product)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };


                // В отдельном потоке добавить в эластик серч продукт
                using (var MyClient = new HttpClient(handler))
                {



                    MyClient.BaseAddress = new Uri(ServiceAdresses.test);
                    var data = JsonConvert.SerializeObject(product);
                    var content = new StringContent(
                        data, Encoding.UTF8, "application/json");

                    var MyResponse = await MyClient.PostAsync("ElasticSearch/AddProduct", content);


                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateElasticProduct(Product product)
        {
            try
            {

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };

                // В отдельном потоке добавить в эластик серч продукт
                using (var MyClient = new HttpClient(handler))
                {
                    MyClient.BaseAddress = new Uri(ServiceAdresses.test);
                    var data = JsonConvert.SerializeObject(product);
                    var content = new StringContent(
                        data, Encoding.UTF8, "application/json");

                    var MyResponse = await MyClient.PostAsync("ElasticSearch/UpdateProduct", content);
                }
            }
            catch (Exception ex)
            {
                var sdfs = 35235;
            }
        }

        #endregion

        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        public async Task AddProduct(Product product)
        {



            try
            {

                context.Product.Add(product);
                context.SaveChanges();


                // В отдельном потоке добавить в эластик серч продукт
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    AddToElasticProduct(product);

                }));

                thread.Start();
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Редактирование продукта
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public async Task EditProduct(Product product)
        {
            var p = context.Product.FirstOrDefault(i => i.Id == product.Id);
            p.Name = product.Name;
            p.Price = product.Price;
            p.Amount = product.Amount;
            p.IdSubCategory = product.IdSubCategory;
            if (!string.IsNullOrEmpty(product.description))
            {
                p.description = product.description;
            }

            // В случае если постер редактируемого продукта не добавлен, то не обновлять
            if (product.Poster != null)
                p.Poster = product.Poster;


            // В отдельном потоке добавить в эластик серч продукт
            Thread thread = new Thread(new ThreadStart(() =>
            {
                UpdateElasticProduct(product);

            }));

            thread.Start();

            context.SaveChanges();
        }

        /// <summary>
        /// Получить продукт по айди
        /// </summary>
        /// <param name="idProduct">Айди продукта</param>
        /// <returns>Продукт</returns>
        public async Task<Product> getProductId(int idProduct)
        {
            Product product = await context.Product.Select(i => new Product()
            {
                // Скидка продукта
                DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),
                description = i.description,
                Id = i.Id,
                Name = i.Name,
                Poster = i.Poster,
                Price = i.Price,
                Amount = i.Amount,
                IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


            }).FirstOrDefaultAsync(i => i.Id == idProduct);

            return product;
        }

        /// <summary>
        /// Метод на выборку из базы данных рандомных номеров продуктов которые сейчас по акции
        /// </summary>
        /// <param name="countTake">Количество товаров</param>
        /// <returns>Рандомные номера продуктов по скидке</returns>
        private async Task<List<int>> GetRandomDiscountsProductIDs(int countTake)
        {
            Random rand = new Random();

            //
            var allDiscountsProduct = await context.Product
                // Все айдишники, товары которые имеют скидки на текущую дату
                .Where(s => s.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).FirstOrDefault() != null)
                .Select(i => i.Id)
                .ToListAsync();

            var randoms = allDiscountsProduct.OrderBy(x => rand.Next()).Take(countTake).ToList();

            return randoms;
        }


        /// <summary>
        /// Выборка рандомных товаров по скидке
        /// </summary>
        /// <param name="countTake">Количество</param>
        /// <returns>Рандомные товары</returns>
        public async Task<List<Product>> GetRandomDiscountProducts(int countTake = 6)
        {
            // Получаем рандомный список товаров по акции
            var randomIds = await GetRandomDiscountsProductIDs(countTake);


            // Выборка
            var randomProducts = await context.Product
                .Where(i => randomIds.Contains(i.Id)) // выборка продуктов добавленных в корзину
                .Include("IdSubCategoryNavigation")
                //.OrderByDescending(i => i.Id)
                .Select(i => new Product()
                {
                    // Скидка продукта
                    DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),
                    description = i.description,
                    Id = i.Id,
                    Name = i.Name,
                    Poster = i.Poster,
                    Price = i.Price,
                    Amount = i.Amount,
                    IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


                })
                .ToListAsync();

            return randomProducts;
        }


        /// <summary>
        /// Выборка случайных продуктов из всей базы данных
        /// </summary>
        /// <param name="countTake"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetRandomProdutsAll(int countTake = 6)
        {
            // Получаем рандомный список товаров из всей бд
            Random rand = new Random();

            // Список айдишников случайных всех продуктов в БД
            var allDiscountsProduct = await context.Product.Select(i => i.Id).ToListAsync();

            allDiscountsProduct.Shuffle();

            var randomIds = allDiscountsProduct.Take(countTake).ToList();


            // Делаем выборку этих продуктов
            var randomProducts = await context.Product
                .Where(i => randomIds.Contains(i.Id)) // выборка продуктов добавленных в корзину
                .Include("IdSubCategoryNavigation")
                //.OrderByDescending(i => i.Id)
                .Select(i => new Product()
                {
                    // Скидка продукта
                    DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),
                    description = i.description,
                    Id = i.Id,
                    Name = i.Name,
                    Poster = i.Poster,
                    Price = i.Price,
                    Amount = i.Amount,
                    IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


                })
                .ToListAsync();

            // Теперь необходимо поменять порядками

            RandFisher.Shuffle(randomProducts);

            return randomProducts;
        }


        /// <summary>
        /// Выборка недавно добавленных продуктов
        /// </summary>
        /// <param name="count">Количество</param>
        /// <returns></returns>
        public async Task<List<Product>> GetNewsProduct(int count)
        {
            int skip = 0;

            if (context.Product.Count() > 5)
            {
                skip = context.Product.Count() - count;
            }

            // Выборка
            var newsProducts = await context.Product.Skip(skip).Take(count)
                .Include("IdSubCategoryNavigation")
                .OrderByDescending(i => i.Id)
                .Select(i => new Product()
                {
                    // Скидка продукта
                    DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),

                    Id = i.Id,
                    Name = i.Name,
                    Poster = i.Poster,
                    Price = i.Price,
                    Amount = i.Amount,
                    description = i.description,
                    IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


                })
                .ToListAsync();

            return newsProducts;
        }

        /// <summary>
        /// Выборка продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>   
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory, int page = 0, int count = 18)
        {
            var products = await context.Product.Where(i => i.IdSubCategory == IdCategory)
                .Skip(page * count).Take(count)
                .Include("IdSubCategoryNavigation")
                .OrderByDescending(i => i.Id)
                .Select(i => new Product()
                {
                    // Скидка продукта
                    DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),
                    description = i.description,
                    Id = i.Id,
                    Name = i.Name,
                    Poster = i.Poster,
                    Price = i.Price,
                    Amount = i.Amount,
                    //IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


                })
                .ToListAsync();

            return products;
        }

        /// <summary>
        /// Поиск продуктов по названию
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <param name="DiscountProduct">Выборка по скидке</param>
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(string name, int page = 0, int count = 18, bool DiscountProduct = false)
        {
            var products = await context.Product
                .Where(i => name != null ? i.Name.Contains(name) : true) // фильтрация по названию
                .Where(i => DiscountProduct == false ? true : i.DiscountProduct.Where(s => s.DateStart < DateTime.Now && s.DateEnd > DateTime.Now).Count() > 0) // Фильтрация по скидкам продукта
                .Skip(page * count).Take(count)
                .Include("IdSubCategoryNavigation")
                .OrderByDescending(i => i.Id)
                .Select(i => new Product()
                {
                    // Скидка продукта
                    DiscountProduct = i.DiscountProduct.Where(i => i.DateStart < DateTime.Now && i.DateEnd > DateTime.Now).ToList(),

                    Id = i.Id,
                    Name = i.Name,
                    Poster = i.Poster,
                    Price = i.Price,
                    Amount = i.Amount,
                    description = i.description,
                    IdSubCategoryNavigation = new SubCategoryProduct() { Id = i.IdSubCategoryNavigation.Id, Name = i.IdSubCategoryNavigation.Name },


                })
                .ToListAsync();

            return products;
        }


        /// <summary>
        /// Проверка на существование продукта
        /// </summary>
        /// <param name="id">Айди продукта</param>
        /// <returns>Возвращает true, если продукт есть в базе данных</returns>
        public async Task<bool> ExistProduct(int id)
        {
            var product = await context.Product.FirstOrDefaultAsync(i => i.Id == id);

            return product != null ? true : false;
        }


        /// <summary>
        /// Поиск продукта по айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>Продукт</returns>
        public async Task<Product> GetProduct(int id)
        {
            return await context.Product.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}

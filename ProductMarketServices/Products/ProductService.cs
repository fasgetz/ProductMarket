using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductMarketModels;
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
                // В отдельном потоке добавить в эластик серч продукт
                using (var MyClient = new HttpClient())
                {

                    MyClient.BaseAddress = new Uri("https://localhost:44330/");
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
                // В отдельном потоке добавить в эластик серч продукт
                using (var MyClient = new HttpClient())
                {

                    MyClient.BaseAddress = new Uri("https://localhost:44330/");
                    var data = JsonConvert.SerializeObject(product);
                    var content = new StringContent(
                        data, Encoding.UTF8, "application/json");

                    var MyResponse = await MyClient.PostAsync("ElasticSearch/UpdateProduct", content);
                }
            }
            catch (Exception ex)
            {

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
                using (context = new ProductMarketContext())
                {
                    context.Product.Add(product);
                    await context.SaveChangesAsync();

                    // В отдельном потоке добавить в эластик серч продукт
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        AddToElasticProduct(product);

                    }));

                    thread.Start();

                }
            }
            catch (Exception)
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
        /// Выборка недавно добавленных продуктов
        /// </summary>
        /// <param name="count">Количество</param>
        /// <returns></returns>
        public async Task<List<Product>> GetNewsProduct(int count)
        {
            // Выборка
            var newsProducts = await context.Product.Skip(context.Product.Count() - count).Take(count)
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
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(string name, int page = 0, int count = 18)
        {
            var products = await context.Product.Where(i => name != null ? i.Name.Contains(name) : true)
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

using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductMarketContext context;

        public ProductService(ProductMarketContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        public async Task AddProduct(Product product)
        {

            context.Product.Add(product);
            context.SaveChanges();


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
        /// Выборка ВСЕХ продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>   
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory)
        {
            var products = await context.Product.Where(i => i.IdSubCategory == IdCategory)
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
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(string name)
        {
            var products = await context.Product.Where(i => i.Name.Contains(name))
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
        /// Выборка продуктов по категории в количестве (для страничной навигации)
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <param name="TakeCount">Взять из выборки</param>
        /// <param name="SkipCount">Пропуск из выборки</param>        
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory, int TakeCount = 0, int SkipCount = 0)
        {
            var products = await context.Product.Where(i => i.IdSubCategory == IdCategory).Skip(SkipCount).Take(TakeCount)
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

using Microsoft.VisualBasic.CompilerServices;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Products
{
    public interface IProductService
    {
        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        public Task AddProduct(Product product);

        /// <summary>
        /// Редактирование продукта
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public Task EditProduct(Product product);


        /// <summary>
        /// Выборка недавно добавленных продуктов
        /// </summary>
        /// <param name="count">Количество</param>
        /// <returns></returns>
        public Task<List<Product>> GetNewsProduct(int count);


        /// <summary>
        /// Выборка продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>   
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <returns>Продукты</returns>
        Task<List<Product>> GetProducts(short IdCategory, int page = 0, int count = 18);





        /// <summary>
        /// Проверка на существование продукта
        /// </summary>
        /// <param name="id">Айди продукта</param>
        /// <returns>Возвращает true, если продукт есть в базе данных</returns>
        Task<bool> ExistProduct(int id);


        /// <summary>
        /// Поиск продукта по айди
        /// </summary>
        /// <param name="id">айди</param>
        /// <returns>Продукт</returns>
        Task<Product> GetProduct(int id);


        /// <summary>
        /// Поиск продуктов по названию
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <returns>Продукты</returns>
        public Task<List<Product>> GetProducts(string name, int page = 0, int count = 18);


    }
}

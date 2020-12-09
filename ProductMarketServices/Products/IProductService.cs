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
        /// Выборка ВСЕХ продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <param name="IdCategory">Категория продуктов</param>        
        /// <returns>Продукты</returns>
        Task<List<Product>> GetProducts(short IdCategory);


        /// <summary>
        /// Выборка продуктов по категории в количестве (для страничной навигации)
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <param name="TakeCount">Взять из выборки</param>
        /// <param name="SkipCount">Пропуск из выборки</param>        
        /// <returns>Продукты</returns>
        Task<List<Product>> GetProducts(short IdCategory, int TakeCount = 0, int SkipCount = 0);


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


    }
}

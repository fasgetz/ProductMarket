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



    }
}

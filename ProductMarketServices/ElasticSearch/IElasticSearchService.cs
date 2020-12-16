using ProductMarketModels;
using ProductMarketModelsElastic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ElasticSearch
{
    public interface IElasticSearchService
    {

        /// <summary>
        /// Множественное добавление
        /// </summary>
        /// <param name="products">Список продуктов</param>
        /// <param name="indexName">Индекс</param>
        /// <returns></returns>
        public Task AddManyAsync(List<ProductSuggest> products, string indexName = null);

        /// <summary>
        /// Выборка продуктов из эластика с автодополнением
        /// </summary>
        /// <param name="searchText">Искомый текст</param>
        /// <param name="countTake">Количество элементов которых взять</param>
        /// <returns>Список продуктов с автодополнением</returns>
        public Task<List<ProductSuggest>> GetProductSuggests(string searchText, int countTake = 5);

        /// <summary>
        /// Выборка всех документов
        /// </summary>
        /// <returns>Все продукты из эластика</returns>
        public Task<List<Product>> GetAllProducts();

        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public Task UpdateProduct(Product product);

        /// <summary>
        /// Сохранить продукт (или обновить)
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public Task SaveSingleAsync(Product product);

    }
}

using Nest;
using ProductMarketModels;
using ProductMarketModelsElastic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ElasticSearch
{

    public class ElasticSearchService : IElasticSearchService
    {
        #region Свойства

        private readonly IElasticClient _elasticClient;

        #endregion

        public ElasticSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        #region Методы сервиса

        /// <summary>
        /// Множественное добавление
        /// </summary>
        /// <param name="products">Список продуктов</param>
        /// <param name="indexName">Индекс</param>
        /// <returns></returns>
        public async Task AddManyAsync(List<ProductSuggest> products, string indexName = null)
        {
            await _elasticClient.IndexManyAsync(products, indexName);
        }

        /// <summary>
        /// Выборка продуктов из эластика с автодополнением
        /// </summary>
        /// <param name="searchText">Искомый текст</param>
        /// <param name="countTake">Количество элементов которых взять</param>
        /// <returns>Список продуктов с автодополнением</returns>
        public async Task<List<ProductSuggest>> GetProductSuggests(string searchText, int countTake = 5)
        {
            ISearchResponse<ProductSuggest> searchResponse = await _elasticClient.SearchAsync<ProductSuggest>(s => s
                         .Index("products")
                         .Suggest(su => su
                              .Completion("suggestions", c => c
                                   .Field(f => f.Suggest)
                                   .Prefix(searchText)
                                   .Fuzzy(f => f
                                       .Fuzziness(Fuzziness.Auto)
                                   )
                                   .Size(countTake))
                                 ));

            var suggests = from suggest in searchResponse.Suggest["suggestions"]
                           from option in suggest.Options
                           select new ProductSuggest
                           {
                               id = option.Source.id,
                               name = option.Source.name,
                               idSubCategory = option.Source.idSubCategory
                           };

            return suggests.ToList();
        }

        /// <summary>
        /// Выборка всех документов
        /// </summary>
        /// <returns>Все продукты из эластика</returns>
        public async Task<List<Product>> GetAllProducts()
        {
            // Выборка всех элементов
            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Index<Product>()
                ).ConfigureAwait(false);


            return searchResponse.Documents.ToList();
        }

        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public async Task UpdateProduct(Product product)
        {
            // формируем продукт с suggest'om для изменения запроса
            ProductSuggest suggest = new ProductSuggest()
            {
                id = product.Id,
                idSubCategory = product.IdSubCategory,
                name = product.Name,
                Suggest = new CompletionField()
                {
                    Input = new string[] { product.Name }
                }
            };

            // Обновляем в эластике
            await _elasticClient.UpdateAsync<ProductSuggest>(suggest, u => u.Doc(suggest));
        }

        /// <summary>
        /// Сохранить продукт (или обновить)
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <returns></returns>
        public async Task SaveSingleAsync(Product product)
        {
            ProductSuggest pr = new ProductSuggest()
            {
                id = product.Id,
                name = product.Name,
                idSubCategory = product.IdSubCategory,
                Suggest = new CompletionField()
                {
                    Input = new string[] { $"{product.Name}" }
                }
            };

            //var asyncIndexResponse = await client.IndexDocumentAsync(person);
            var response = await _elasticClient.IndexDocumentAsync<ProductSuggest>(pr);
        }

        #endregion

    }
}

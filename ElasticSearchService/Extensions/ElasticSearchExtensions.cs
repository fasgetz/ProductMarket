using ProductMarketModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using ProductMarketModelsElastic.Models;
using ProductMarketModels.Constants;

namespace ElasticSearchService.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = ServiceAdresses.ElasticSearch;
            var defaultIndex = "products";

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Product>(m => m
                    .Ignore(p => p.ProductsInOrder)
                    .Ignore(p => p.Poster)
                    .Ignore(p => p.IdSubCategoryNavigation)
                    .Ignore(p => p.IdFabricatorNavigation)
                    .Ignore(p => p.Price)
                    .Ignore(p => p.Amount)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<ProductSuggest>(x => x.AutoMap().Properties(ps => ps.Completion(c => c.Name(p => p.Suggest)))
                )
            );
        }
    }
}

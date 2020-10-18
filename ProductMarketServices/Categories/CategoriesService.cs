using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Categories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ProductMarketContext context;

        public CategoriesService(ProductMarketContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Выборка категорий и субкатегорий с количеством продуктов в них
        /// </summary>
        /// <returns>Выборка категорий и субкатегорий с количеством продуктов в них</returns>
        public async Task<List<Category>> GetCategoriesProducts()
        {
            var query = await context.CategoryProduct
                .Include("SubCategoryProduct")
                .Select(i =>
                    new Category()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Poster = i.Poster,
                        SubCategoryProduct = i.SubCategoryProduct.Select(r => new SubCategoryProducts
                        {
                            Id = r.Id,
                            IdCategory = r.IdCategory,
                            Name = r.Name,
                            Poster = r.Poster,
                            CountProducts = r.Product.Count
                        }).ToList()
                    }
                ).ToListAsync();

            return query;
        }


    }
}

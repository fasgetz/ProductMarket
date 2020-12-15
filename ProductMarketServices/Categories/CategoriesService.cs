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
        /// Добавление категории товара
        /// </summary>
        /// <param name="category">Категория</param>
        /// <returns></returns>
        public async Task AddCategory(Category category)
        {
            context.CategoryProduct.Add
                (
                    new CategoryProduct()
                    {
                        Name = category.Name,
                        Poster = category.Poster
                    }
                );
            context.SaveChanges();
        }

        public async Task AddSubCategory(SubCategoryProduct SubCategory)
        {
            context.SubCategoryProduct.Add(SubCategory);

            context.SaveChanges();
        }

        public async Task DeleteCategory(CategoryProduct category)
        {
            var categoryDb = await context.CategoryProduct.FirstOrDefaultAsync(i => i.Id == category.Id);

            context.CategoryProduct.Remove(categoryDb);
            context.SaveChanges();
        }

        public async Task DeleteSubCategory(SubCategoryProduct category)
        {
            var categoryDb = await context.SubCategoryProduct.FirstOrDefaultAsync(i => i.Id == category.Id);

            context.SubCategoryProduct.Remove(categoryDb);
            context.SaveChanges();
        }

        public async Task EditCategory(CategoryProduct category)
        {
            var entity = context.CategoryProduct.FirstOrDefault(i => i.Id == category.Id);

            entity.Name = category.Name;

            if (category.Poster != null)
                entity.Poster = category.Poster;

            context.SaveChanges();

        }

        public async Task EditSubCategory(SubCategoryProduct category)
        {
            var entity = context.SubCategoryProduct.FirstOrDefault(i => i.Id == category.Id);

            entity.Name = category.Name;
            entity.Poster = category.Poster != null ? category.Poster : null;
            entity.IdCategory = category.IdCategory;

            context.SaveChanges();
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

        /// <summary>
        /// Выборка категории, в которой содержатся подкатегории
        /// </summary>
        /// <param name="CategoryId">Айди категории</param>
        /// <returns>Выборка категорий и субкатегорий с количеством продуктов в них</returns>
        public async Task<Category> GetCategoriesProducts(short CategoryId)
        {
            var query = await context.CategoryProduct
                .Include("SubCategoryProduct")
                .Where(i => i.Id == CategoryId)
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
                ).FirstOrDefaultAsync();

            return query;
        }


        /// <summary>
        /// Получить полную информацию о подкатегории
        /// </summary>
        /// <param name="CategoryId">Категория продукта</param>
        /// <param name="addPoster">Необязательный параметр для выборки добавления постера</param>
        /// <returns>Информации о подкатегории продуктов</returns>
        public async Task<SubCategoryProduct> GetSubCategoryProductData(short CategoryId, bool addPoster = false)
        {
            var subCategory = await context.SubCategoryProduct
                .Select(i => new SubCategoryProduct()
                { 
                    Id = i.Id,
                    IdCategory = i.IdCategory,
                    Name = i.Name,
                    // Если выбран параметр добавления постера продукта, то добавить к выборке, иначе не добавлять к выборке
                    Poster = addPoster == true ? i.Poster : null
                })
                .FirstOrDefaultAsync(i => i.Id == CategoryId);


            return subCategory;
        }


    }
}

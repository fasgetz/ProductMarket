﻿using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Categories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Categories
{
    public interface ICategoriesService
    {

        /// <summary>
        /// Выборка категорий и субкатегорий с количеством продуктов в них
        /// </summary>
        /// <returns>Выборка категорий и субкатегорий с количеством продуктов в них</returns>
        Task<List<Category>> GetCategoriesProducts();

        /// <summary>
        /// Выборка категорий и субкатегорий с количеством продуктов в них
        /// </summary>
        /// <param name="CategoryId">Айди категории</param>
        /// <returns>Выборка категорий и субкатегорий с количеством продуктов в них</returns>
        Task<Category> GetCategoriesProducts(short CategoryId);


        /// <summary>
        /// Добавление категории товара
        /// </summary>
        /// <param name="category">Категория</param>
        /// <returns></returns>
        Task AddCategory(Category category);

        /// <summary>
        /// Добавление ПодКатегории товара
        /// </summary>
        /// <param name="SubCategory">Подкатегория</param>
        /// <returns></returns>
        Task AddSubCategory(SubCategoryProduct SubCategory);

    }
}
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
    }
}

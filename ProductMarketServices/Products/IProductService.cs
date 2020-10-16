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
        /// Выборка продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <returns>Продукты</returns>
        Task<List<Product>> GetProducts(short IdCategory);



    }
}

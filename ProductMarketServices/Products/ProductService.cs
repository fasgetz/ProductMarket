using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductMarketContext db;

        public ProductService(ProductMarketContext context)
        {
            this.db = context;
        }


        /// <summary>
        /// Выборка продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory)
        {
            // Выборка
            var products = await db.Product.Where(i => i.IdSubCategory == IdCategory).ToListAsync();


            return products;
        }
    }
}

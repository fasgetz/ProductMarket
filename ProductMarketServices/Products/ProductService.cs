﻿using Microsoft.EntityFrameworkCore;
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
        private readonly ProductMarketContext context;

        public ProductService(ProductMarketContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="product">Продукт</param>
        public async Task AddProduct(Product product)
        {

            context.Product.Add(product);
            context.SaveChanges();


        }


        /// <summary>
        /// Выборка ВСЕХ продуктов по категории
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>   
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory)
        {
            // Выборка
            var products = await context.Product.Where(i => i.IdSubCategory == IdCategory).ToListAsync();


            return products;
        }

        /// <summary>
        /// Выборка продуктов по категории в количестве (для страничной навигации)
        /// </summary>
        /// <param name="IdCategory">Категория продуктов</param>
        /// <param name="TakeCount">Взять из выборки</param>
        /// <param name="SkipCount">Пропуск из выборки</param>        
        /// <returns>Продукты</returns>
        public async Task<List<Product>> GetProducts(short IdCategory, int TakeCount = 0, int SkipCount = 0)
        {
            // Выборка
            var products = await context.Product.Where(i => i.IdSubCategory == IdCategory).Skip(SkipCount).Take(TakeCount).ToListAsync();


            return products;
        }
    }
}
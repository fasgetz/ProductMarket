﻿using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.ProductsDiscount
{
    public class ProductDiscountService : IProductDiscountService
    {
        private ProductMarketContext context;

        public ProductDiscountService(ProductMarketContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Получает все скидки продукта
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>Все скидки продукта</returns>
        public async Task<List<DiscountProduct>> GetDiscountsProduct(int idProduct)
        {
            var discountsProduct = await context.DiscountProduct.Where(i => i.IdProduct == idProduct).OrderByDescending(i => i.Id).ToListAsync();


            return discountsProduct;
        }

        /// <summary>
        /// Добавить продукт
        /// </summary>
        /// <param name="discount"></param>
        public async void AddDiscountProduct(DiscountProduct discount)
        {
            using (context = new ProductMarketContext())
            {
                context.DiscountProduct.Add(discount);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Удаление из базы данных
        /// </summary>
        /// <param name="IdDiscount">Номер акции</param>
        /// <returns>True, если удаление успешно</returns>
        public async Task<bool> RemoveDiscountProduct(int IdDiscount)
        {
            using (context = new ProductMarketContext())
            {
                var dis = await context.DiscountProduct.FirstOrDefaultAsync(i => i.Id == IdDiscount);

                if (dis != null)
                {
                    context.DiscountProduct.Remove(dis);

                    await context.SaveChangesAsync();

                    return true;
                }

                return false;

            }
        }

        /// <summary>
        /// Редактирование продукта
        /// </summary>
        /// <param name="discount">Редактируемая Акция</param>
        public async void EditDiscountProduct(DiscountProduct discount)
        {
            using (context = new ProductMarketContext())
            {
                var dis = await context.DiscountProduct.FirstOrDefaultAsync(i => i.Id == discount.Id);

                if (dis != null)
                {
                    dis.DateEnd = discount.DateEnd;
                    dis.DateStart = discount.DateStart;
                    dis.ProcentDiscount = discount.ProcentDiscount;

                    await context.SaveChangesAsync();
                }



            }
        }
    }
}

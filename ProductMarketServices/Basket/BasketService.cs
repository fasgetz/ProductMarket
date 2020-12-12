using Microsoft.EntityFrameworkCore;
using ProductMarketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarketServices.Basket
{
    public class BasketService
    {
        private readonly ProductMarketContext context;

        public BasketService(ProductMarketContext context)
        {
            this.context = context;
        }



        public async Task<List<Product>> GetProductsFromBasket()
        {
            int[] mas = new int[] { 1, 3, 5, 7 };


            var products = await context.Product.Where(i => mas.Contains(i.Id)).ToListAsync();

            return products;
        }

    }
}

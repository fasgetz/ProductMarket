using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{
    public class GetProductsRespond
    {
        public List<Product> Products { get; set; }


        // Категория продукта
        public SubCategoryProduct categoryProduct { get; set; }


        public GetProductsRespond(List<Product> products, SubCategoryProduct categoryProduct = null)
        {
            this.Products = products;
            this.categoryProduct = categoryProduct;
        }
    }
}

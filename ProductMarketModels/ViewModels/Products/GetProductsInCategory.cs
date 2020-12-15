using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.ViewModels.Products
{
    public partial class GetProductsInCategory
    {
        public List<Product> products { get; set; }
        public SubCategoryProduct category { get; set; }
    }
}

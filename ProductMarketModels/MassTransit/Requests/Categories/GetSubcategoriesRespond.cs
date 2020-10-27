using ProductMarketModels.MassTransit.Requests.Categories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Categories
{
    public class GetSubcategoriesRespond
    {
        public Category Category { get; private set; }        

        public GetSubcategoriesRespond(Category Category)
        {
            this.Category = Category;
        }
    }
}

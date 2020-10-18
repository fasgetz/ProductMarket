using ProductMarketModels.MassTransit.Requests.Categories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Categories
{
    public class GetCategoriesRespond
    {
        public List<Category> Categories { get; private set; }

        public GetCategoriesRespond(List<Category> categories)
        {
            this.Categories = categories;
        }
    }
}

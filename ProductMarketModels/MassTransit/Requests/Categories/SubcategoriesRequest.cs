using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Categories
{
    public class SubcategoriesRequest
    {
        public short IdCategory { get; private set; }

        public SubcategoriesRequest(short IdCategory)
        {
            this.IdCategory = IdCategory;
        }
    }
}

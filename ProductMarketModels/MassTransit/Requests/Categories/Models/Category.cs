using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Categories.Models
{
    public class Category
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public byte[] Poster { get; set; }

        public virtual ICollection<SubCategoryProducts> SubCategoryProduct { get; set; }
    }
}

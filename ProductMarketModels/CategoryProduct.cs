using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class CategoryProduct
    {
        public CategoryProduct()
        {
            SubCategoryProduct = new HashSet<SubCategoryProduct>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public byte[] Poster { get; set; }

        public virtual ICollection<SubCategoryProduct> SubCategoryProduct { get; set; }
    }
}

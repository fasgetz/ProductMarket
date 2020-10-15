using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class SubCategoryProduct
    {
        public SubCategoryProduct()
        {
            Product = new HashSet<Product>();
        }

        public short Id { get; set; }
        public short? IdCategory { get; set; }
        public string Name { get; set; }
        public byte[] Poster { get; set; }

        public virtual CategoryProduct IdCategoryNavigation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}

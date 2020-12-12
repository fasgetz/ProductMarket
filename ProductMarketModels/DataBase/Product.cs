using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class Product
    {
        public Product()
        {
            DiscountProduct = new HashSet<DiscountProduct>();
            ProductsInOrder = new HashSet<ProductsInOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Amount { get; set; }
        public short IdSubCategory { get; set; }
        public short? IdFabricator { get; set; }

        public virtual ICollection<DiscountProduct> DiscountProduct { get; set; }
        public byte[] Poster { get; set; }

        public virtual Fabricator IdFabricatorNavigation { get; set; }
        public virtual SubCategoryProduct IdSubCategoryNavigation { get; set; }

        public virtual ICollection<ProductsInOrder> ProductsInOrder { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ProductMarketModels
{
    public partial class Fabricator
    {
        public Fabricator()
        {
            Product = new HashSet<Product>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}

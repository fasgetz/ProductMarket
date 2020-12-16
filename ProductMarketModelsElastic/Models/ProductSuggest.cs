using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModelsElastic.Models
{
    public partial class ProductSuggest
    {
        public int id { get; set; }
        public string name { get; set; }
        public short idSubCategory { get; set; }
        public CompletionField Suggest { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Categories.Models
{
    public class SubCategoryProducts
    {
        public short Id { get; set; }
        public short? IdCategory { get; set; }
        public string Name { get; set; }
        public byte[] Poster { get; set; }

        public int CountProducts { get; set; }
    }
}

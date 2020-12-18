using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Models.ViewModels.Search
{
    
    /// <summary>
    /// Модель, которая содержит поисковой запрос
    /// </summary>
    public partial class SearchData
    {
        public string name { get; set; }
        public int idSubCategory { get; set; }
        public int page { get; set; }
        public int count { get; set; }
        public bool discount { get; set; }
    }
}

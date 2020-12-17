using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Models.ViewModels.Search;

namespace WebSiteProductMarket.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult SearchData()
        {
            return View();
        }


        /// <summary>
        /// Поиск по категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("category/search")]
        public IActionResult SearchCategory(int category, int page = 0, int count = 18)
        {
            SearchData data = new SearchData()
            {
                idSubCategory = category,
                page = page,
                count = count
            };


            return View(data);
        }


        /// <summary>
        /// Поиск по категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("search")]
        public IActionResult SearchName(string name, int page = 0, int count = 18)
        {
            SearchData data = new SearchData()
            {
                name = name,
                page = page,
                count = count
            };

            return View("SearchName", data);
        }
    }
}

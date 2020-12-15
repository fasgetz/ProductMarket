using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [Route("search/category/{category?}")]
        public IActionResult SearchCategory(int? category)
        {
            return View(category);
        }


        /// <summary>
        /// Поиск по категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("search")]
        public IActionResult SearchName(string name)
        {
            return View();
        }
    }
}

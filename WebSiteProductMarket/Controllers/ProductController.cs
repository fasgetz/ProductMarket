using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers
{
    public class ProductController : Controller
    {
        public ProductController()
        {

        }


        public IActionResult About(int id)
        {
            return View("About", id);
        }
    }
}

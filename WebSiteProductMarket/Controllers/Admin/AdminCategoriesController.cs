using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers.Admin
{
    [Authorize(Roles = "Администратор")]
    public class AdminCategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCategory()
        {
            return PartialView();
        }

        public IActionResult AddSubCategory()
        {
            return PartialView();
        }

        public IActionResult EditCategory(int id, string name)
        {
            ViewBag.name = name;
            ViewBag.id = id;

            return PartialView();
        }
    }
}

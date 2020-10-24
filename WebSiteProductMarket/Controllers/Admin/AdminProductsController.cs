using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers.Admin
{

    [Authorize(Roles = "Администратор")]
    public class AdminProductsController : Controller
    {
        private readonly ILogger<AdminProductsController> _logger;

        public AdminProductsController(ILogger<AdminProductsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}

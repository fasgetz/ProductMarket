using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductMarketModels.ViewModels.Admin.ProductsController;

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


        public IActionResult EditProduct(EditProductViewModel vm)
        {
            return PartialView(vm);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}

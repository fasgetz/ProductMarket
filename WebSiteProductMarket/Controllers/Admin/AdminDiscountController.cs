using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarketModels.ViewModels.Admin.ProductsController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers.Admin
{

    [Authorize(Roles = "Администратор")]
    public class AdminDiscountController : Controller
    {
        public AdminDiscountController()
        {

        }

        public IActionResult AddDiscount(EditProductViewModel vm)
        {
            return PartialView(vm);
        }
    }
}

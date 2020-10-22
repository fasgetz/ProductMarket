using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Identity;
using WebSiteProductMarket.Models.ViewModels.Account;

namespace WebSiteProductMarket.Components.Account
{
    /// <summary>
    /// Компонент для регистрации пользователя в Identity ASP Net Core
    /// </summary>
    [ViewComponent]
    public class LoginAccountComponent : ViewComponent
    {


        public LoginAccountComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View("Login", new LoginUserViewModel());
        }
    }
}

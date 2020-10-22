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
    public class RegisterAccountComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterAccountComponent(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IViewComponentResult Invoke()
        {


            return View("Register", new RegisterUserViewModel());
        }
    }
}

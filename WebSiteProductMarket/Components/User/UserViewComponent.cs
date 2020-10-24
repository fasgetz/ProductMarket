using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Components.User
{
    /// <summary>
    /// Компонент для возвращения модели юзера
    /// </summary>
    [ViewComponent]
    public class UserViewComponent : ViewComponent
    {
        private readonly UserManager<WebSiteProductMarket.Identity.User> _userManager;

        public UserViewComponent(UserManager<WebSiteProductMarket.Identity.User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null)
                return null;

            return View("AboutUser", user);
        }

    }
}

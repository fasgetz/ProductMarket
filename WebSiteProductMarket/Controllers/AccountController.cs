using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebSiteProductMarket.Identity;
using WebSiteProductMarket.Models.ViewModels.Account;

namespace WebSiteProductMarket.Controllers
{


    public class AccountController : Controller
    {
        async Task<string> GenerateJwtToken(string UserName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var user = await _userManager.FindByEmailAsync(UserName);
            var userRoles = await _userManager.GetRolesAsync(user);
            //var userRole = await _roleManager.(UserName);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Добавляем роли в привязку
            foreach (var item in userRoles)
            {
                claims.Add(new Claim("role", item));
            }


            var token = new JwtSecurityToken(audience: "https://localhost:44336/",
                issuer: "https://localhost:44336/",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }


        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Email = vm.Email,
                    UserName = vm.Email
                };

                

                var result = await _userManager.CreateAsync(user, vm.Password);

                // Если успешная регистрация
                if (result.Succeeded)
                {
                    // Установка куки
                    await _signInManager.SignInAsync(user, false);

                    // Авторизоваться в апи
                    var token = GenerateJwtToken(vm.Email).Result;

                    HttpContext.Response.Cookies.Append("token", token,
                    new CookieOptions
                    {
                        MaxAge = TimeSpan.FromDays(30),
                        Secure = false
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return View("Register", vm);
        }



        [HttpPost]
        // Метод контроллера для авторизацияя
        public async Task<IActionResult> Login(LoginUserViewModel vm)
        {
            

            if (ModelState.IsValid)
            {
                // Авторизация
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);

                // Если успешная авторизация, то 
                if (result.Succeeded)
                {
                    // Авторизоваться в апи
                    var token = GenerateJwtToken(vm.Email).Result;

                    HttpContext.Response.Cookies.Append("token", token,
                    new CookieOptions
                    {                        
                        MaxAge = TimeSpan.FromDays(30),
                        Secure = false
                    });

                    
                    string url = Request.Headers["Referer"].ToString();
                    //Your code to store data     
                    return PartialView("Controllers/Account/_AuthSuccessfully", url);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный email и (или) пароль");

                }
            }

            return PartialView("Controllers/Account/Login", vm);
        }

        /// <summary>
        /// Метод для выхода из аккаунта
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            HttpContext.Response.Cookies.Delete("token");

            return RedirectToAction("Index", "Home");
        }

    }
}

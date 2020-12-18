using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebSiteProductMarket.Models.ViewModels.Profile;
using WebSiteProductMarket.Service;

namespace WebSiteProductMarket.Controllers
{

    [Authorize]
    [Route("Profile")]
    public class UserProfileController : Controller
    {

        private readonly IProfileService service;

        public UserProfileController(IProfileService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("ProfileData")]
        public async Task<IActionResult> ProfileData()
        {
            var user = await service.GetUser(User.Identity.Name);

            IndexProfileViewModel vm = new IndexProfileViewModel()
            {

                user = user
            };

            return PartialView("ProfileData", vm);
        }



        [HttpPost("RemoveAvatar")]
        public async Task<IActionResult> DeleteAvatar()
        {
            var deleted = await service.RemoveUserAvatar(User.Identity.Name);

            if (!deleted)
                return BadRequest();

            return Redirect($"/Profile/");
        }


        [HttpPost("load")]
        public async Task<IActionResult> LoadAvatar()
        {
            try
            {
                // Получаем файл
                var file = Request.Form.Files.FirstOrDefault();


                byte[] imageData = null;

                //считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)file.Length);
                }

                // Обновляем аватар
                var updated = await service.UpdateAvatar(User.Identity.Name, imageData);

                if (!updated)
                    return BadRequest();
            }
            catch (Exception ex)
            {

            }


            return Ok("Success");
        }


        [HttpPost("SaveProfile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(IndexProfileViewModel vm)
        {
            vm.email = User.Identity.Name;
            var updated = await service.UpdateUser(vm);

            if (updated == true)
                return Redirect($"/Profile/");

            return View();
        }
    }
}


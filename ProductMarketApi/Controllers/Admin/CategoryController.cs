using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Categories.Models;
using ProductMarketModels.ViewModels.Admin.CategoryController;
using ProductMarketModels.ViewModels.Admin.ProductsController;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.Admin)]
    public class CategoryController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;

        public CategoryController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }





        //[HttpPost("Add")]
        //public async Task<IActionResult> AddCategory([FromForm] AddCategoryViewModel category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //    //await mPublishEndpoint.Publish(product);
        //    return Ok("Success");
        //}

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] imageData = null;

            //считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(categoryVM.file.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)categoryVM.file.Length);
            }

            Category category = new Category()
            {
                Name = categoryVM.Name,
                Poster = imageData
            };


            await mPublishEndpoint.Publish(category);
            return Ok("Success");
        }

        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory([FromForm] AddSubCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] imageData = null;

            //считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(categoryVM.file.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)categoryVM.file.Length);
            }

            SubCategoryProduct subcategory = new SubCategoryProduct()
            {
                Name = categoryVM.Name,
                Poster = imageData,
                IdCategory = (short)categoryVM.IdCategory
            };


            await mPublishEndpoint.Publish(subcategory);
            return Ok("Success");
        }
    }



}

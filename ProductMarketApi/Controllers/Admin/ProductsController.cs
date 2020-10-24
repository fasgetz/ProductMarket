using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.ProductsController;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.Admin)]
    public class ProductsController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;

        public ProductsController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }





        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            //await mPublishEndpoint.Publish(product);
            return Ok("Success");
        }

        [HttpPost("AddFile")]
        public async Task<IActionResult> AddFile([FromForm] AddProductViewModel prod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] imageData = null;

            //считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(prod.file.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)prod.file.Length);
            }

            Product product = new Product()
            {
                Amount = prod.Amount,
                IdSubCategory = prod.idSubCategoryProduct,
                Name = prod.Name,
                Price = prod.price,
                Poster = imageData
            };


            await mPublishEndpoint.Publish(product);
            return Ok("Success");
        }
    }



}

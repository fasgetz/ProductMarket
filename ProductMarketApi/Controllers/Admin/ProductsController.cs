using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.ProductsController;
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




        [HttpPost("EditProduct")]
        public async Task<IActionResult> Edit([FromForm] EditProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Если загрузили файл
            if (vm.file != null)
                //считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(vm.file.OpenReadStream()))
                {
                    vm.image = binaryReader.ReadBytes((int)vm.file.Length);
                    vm.file = null;
                }


            await mPublishEndpoint.Publish(vm);
            return Ok("Success");
        }


        [HttpPost("AddProduct")]
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

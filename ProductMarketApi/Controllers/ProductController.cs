using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;

        public ProductController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }


        [HttpPost("AddProduct")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> AddProduct(Product product)
        {
            await mPublishEndpoint.Publish(product);
            return Ok("Success");
        }

        [Authorize(Policy = Policies.Admin)]
        [HttpGet("get")]
        public async Task<List<Product>> GetProducts(short CategoryProduct)
        {
            
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductsRequest>(serviceAddress);


            var response = await client.GetResponse<GetProductsRespond>(new GetProductsRequest() { IdCategoryProduct = CategoryProduct });

            return response.Message.Products;
        }
    }
}

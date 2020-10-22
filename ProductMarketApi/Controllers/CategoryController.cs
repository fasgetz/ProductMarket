using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Categories;
using ProductMarketModels.MassTransit.Requests.Categories.Models;
using ProductMarketModels.MassTransit.Requests.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;


        public CategoryController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }



        [HttpGet("get")]
        public async Task<List<Category>> GetCategories()
        {
            //var response = await mPublishEndpoint.Request<GetProductsRequest, GetProductsRespond>(new GetProductsRequest { IdCategoryProduct = CategoryProduct });
            //var tokens = Request.Cookies;
            //var token = Request.Cookies[".AspNetCore.Application.Id"];

            //var headers = Request.Headers["token"];
            

            var serviceAddress = new Uri("rabbitmq://localhost/CategoriesQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductsRequest>(serviceAddress);


            var response = await client.GetResponse<GetCategoriesRespond>(new GetProductsRequest());
            

            return response.Message.Categories;
        }

        
        [HttpGet("get2")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // <-- That's all, folks! :)*
        public async Task<List<Category>> GetCategories2()
        {
            //var response = await mPublishEndpoint.Request<GetProductsRequest, GetProductsRespond>(new GetProductsRequest { IdCategoryProduct = CategoryProduct });


            var serviceAddress = new Uri("rabbitmq://localhost/CategoriesQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductsRequest>(serviceAddress);


            var response = await client.GetResponse<GetCategoriesRespond>(new GetProductsRequest());


            return response.Message.Categories;
        }
    }
}

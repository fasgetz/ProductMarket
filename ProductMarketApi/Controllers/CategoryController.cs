using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProductMarketModels.MassTransit.Requests.Categories;
using ProductMarketModels.MassTransit.Requests.Categories.Models;
using ProductMarketModels.MassTransit.Requests.Products;
using System;
using System.Collections.Generic;
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

            var serviceAddress = new Uri("rabbitmq://localhost/CategoriesQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetSubcategoriesRequest>(serviceAddress);


            var response = await client.GetResponse<GetCategoriesRespond>(new GetSubcategoriesRequest());

            var objects = response.Message.Categories;

            //var newObjects = objects.Select(i => new CategoryDTO
            //{
            //    Id = i.Id,
            //    Name = i.Name,
            //    Poster = i.Poster != null ? Convert.ToBase64String(i.Poster) : null

            //}).ToList();

            return objects;
        }

        [HttpGet("getCategory")]
        public async Task<Category> GetCategory(short IdCategory)
        {

            var serviceAddress = new Uri("rabbitmq://localhost/CategoriesQueue");
            var client = mPublishEndpoint.CreateRequestClient<SubcategoriesRequest>(serviceAddress);


            var response = await client.GetResponse<GetSubcategoriesRespond>(new SubcategoriesRequest(IdCategory));
            
            return response.Message.Category;
        }


        [HttpGet("get2")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // <-- That's all, folks! :)*
        public async Task<List<Category>> GetCategories2()
        {
            
            var serviceAddress = new Uri("rabbitmq://localhost/CategoriesQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetSubcategoriesRequest>(serviceAddress);


            var response = await client.GetResponse<GetCategoriesRespond>(new GetSubcategoriesRequest());


            return response.Message.Categories;
        }
    }
}

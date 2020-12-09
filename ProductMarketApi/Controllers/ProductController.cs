using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketModels.MassTransit.Requests.Products.Responds;
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


        
        [HttpGet("get")]
        public async Task<List<Product>> GetProducts(short CategoryProduct)
        {
            
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetSubcategoriesRequest>(serviceAddress);


            var response = await client.GetResponse<GetProductsRespond>(new GetSubcategoriesRequest() { IdCategoryProduct = CategoryProduct });

            return response.Message.Products;
        }


        [HttpGet("newsProduct")]
        public async Task<List<Product>> GetProducts(int count)
        {

            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetNewsProductsRequest>(serviceAddress);

            var response = await client.GetResponse<GetProductsRespond>(new GetNewsProductsRequest() { countProducts = count });

            return response.Message.Products;
        }


        /// <summary>
        /// Поиск продукта по айди
        /// </summary>
        /// <param name="idProduct">Айди продукта</param>
        /// <returns>True, если продукт есть в базе данных</returns>
        [HttpGet("ExistProduct")]
        public async Task<bool> exist(int idProduct)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<IdProductRequest>(serviceAddress);

            var response = await client.GetResponse<ExistProductRespond>(new IdProductRequest() { idProduct = idProduct });

            return response.Message.existProduct;
        }
    }
}

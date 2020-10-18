﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddProduct(Product product)
        {
            await mPublishEndpoint.Publish(product);
            return Ok("Success");
        }


        [HttpGet("get")]
        public async Task<List<Product>> GetProducts(short CategoryProduct)
        {
            //var response = await mPublishEndpoint.Request<GetProductsRequest, GetProductsRespond>(new GetProductsRequest { IdCategoryProduct = CategoryProduct });

            
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductsRequest>(serviceAddress);


            var response = await client.GetResponse<GetProductsRespond>(new GetProductsRequest() { IdCategoryProduct = CategoryProduct });

            return response.Message.Products;
        }
    }
}

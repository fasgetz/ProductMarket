﻿using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketModels.MassTransit.Requests.Products.Responds;
using ProductMarketModels.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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


        /// <summary>
        /// Получить продукты по названию
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <param name="DiscountProduct">Выборка по скидке</param>        
        /// <returns>Продукты</returns>
        [HttpGet("searchName")]
        public async Task<List<Product>> GetProducts(string name = null, int page = 0, int count = 18, bool discount = false)
        {
            if (name != null)
                name = name.Replace("&quot;", "\"");

            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductsNameRequest>(serviceAddress);


            var response = await client.GetResponse<GetProductsRespond>(new GetProductsNameRequest() { name = name, page = page, count = count, discount = discount });

            return response.Message.Products;
        }

        /// <summary>
        /// Получить продукты по категории
        /// </summary>
        /// <param name="CategoryProduct">Категория продукта</param>
        /// <param name="page">Страница</param>
        /// <param name="counts">Количество итемов, которые вывести</param>
        /// <returns>Продукты</returns>
        [HttpGet("category")]
        public async Task<GetProductsInCategory> GetProducts(short CategoryProduct, int page = 0, int count = 18)
        {
            
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetSubcategoriesRequest>(serviceAddress);


            var response = await client.GetResponse<GetProductsRespond>(new GetSubcategoriesRequest() { IdCategoryProduct = CategoryProduct, page = page, count = count });
            
            var data = new GetProductsInCategory()
            {
                products = response.Message.Products,
                category = response.Message.categoryProduct
            };


            return data;
        }


        /// <summary>
        /// Выборка рандомно добавленных товаров
        /// </summary>
        /// <param name="count">Количество</param>
        /// <param name="discount">Выборка рандомных товаров только у тех, у которых есть скидка</param>
        /// <returns>Товары</returns>
        [HttpGet("randomProducts")]
        public async Task<List<Product>> GetProductsRandom(int count, bool discount = false)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetRandomProductsRequest>(serviceAddress);

            var response = await client.GetResponse<GetProductsRespond>(new GetRandomProductsRequest() { countProducts = count, discount = discount });

            return response.Message.Products;
        }

        /// <summary>
        /// Выборка недавно добавленных товаров
        /// </summary>
        /// <param name="count">Количество</param>
        /// <returns>Товары</returns>
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


        /// <summary>
        /// Поиск продукта по айди
        /// </summary>
        /// <param name="idProduct">Айди продукта</param>
        /// <returns>Продукт, если продукт есть в базе данных</returns>
        [HttpGet("GetProductId")]
        public async Task<Product> productGetId(int idProduct)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<GetProductId>(serviceAddress);

            var response = await client.GetResponse<Product>(new GetProductId() { idProduct = idProduct });

            return response.Message;
        }
    }
}

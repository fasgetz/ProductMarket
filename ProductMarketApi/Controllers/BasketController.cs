using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Basket;
using ProductMarketModels.MassTransit.Requests.Categories;
using ProductMarketModels.MassTransit.Responds.Basket;
using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers
{

    /// <summary>
    /// Контроллер корзины покупок
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;


        public BasketController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        [HttpPost("AddOrder")]
        [Authorize]
        public async Task<IActionResult> orderAdd(OrderBasket basket)
        {
            if (basket.basket.products.Count == 0)
                return BadRequest("Корзина товаров пустая");

            basket.userName = User.Claims.FirstOrDefault()?.Value;

            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<orderBasketRequest>(serviceAddress);


            var response = await client.GetResponse<getOrderRespond>(new orderBasketRequest() { basket = basket });

            return new JsonResult(response.Message.order);
        }


        /// <summary>
        /// Получить продукты в корзине
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>        
        [HttpPost("getBasketProducts")]
        public async Task<Basket> basketGet(Basket basket)
        {

            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<getBasketProductsRequest>(serviceAddress);


            var response = await client.GetResponse<getBasketProductsRespond>(new getBasketProductsRequest() { basket = basket });


            return response.Message.basket;
        }
    }
}

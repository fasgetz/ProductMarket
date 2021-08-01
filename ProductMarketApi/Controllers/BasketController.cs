using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Basket;
using ProductMarketModels.MassTransit.Requests.Categories;
using ProductMarketModels.MassTransit.Requests.PayPal;
using ProductMarketModels.MassTransit.Responds.Basket;
using ProductMarketModels.MassTransit.Responds.PayPal;
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
        /// Оплата через PayPal
        /// </summary>
        /// <param name="basket"></param>
        /// <returns>Генерированная ссылка для оплаты PayPal</returns>
        [HttpPost("PayPalPayment")]
        [Authorize]
        public async Task<IActionResult> paymentPayPal(OrderBasket basket)
        {
            if (basket.basket.products.Count == 0)
                return BadRequest("Корзина товаров пустая");

            basket.userName = User.Claims.FirstOrDefault()?.Value;

            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<orderBasketRequest>(serviceAddress);


            var response = await client.GetResponse<GetUrlPayment>(new orderBasketRequest() { basket = basket });
            // После получения корзины необходимо сформировать платежную корзину в пайпел и вернуть ссылку клиенту для оплаты

            if (string.IsNullOrEmpty(response.Message.url))
            {
                return BadRequest("Payment PalPal not created!");
            }

            return Ok(response.Message.url);
        }


        /// <summary>
        /// Подтверждение платежа PayPal
        /// </summary>
        /// <param name="payment">Данные платежа</param>
        /// <returns>True, в случае успеха платежа</returns>
        [HttpPost("PayPalExecute")]
        public async Task<IActionResult> executePayPal(ExecutePaymentRequest payment)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<ExecutePaymentRequest>(serviceAddress);

            var response = await client.GetResponse<ExecutePaymentRespond>(payment);


            if (response.Message.successPayment == false)
            {
                return BadRequest("Оплата неуспешна");
            }

            return Ok(response.Message.successPayment);
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


        /// <summary>
        /// Получить заказы пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Список заказов</returns>
        [HttpGet("GetUserOrders")]
        public async Task<IActionResult> GetUserOrders(string user)
        {
            try
            {
                var userAuth = User.Claims.FirstOrDefault().Value;


                // Если текущий пользователь авторизован
                // и является тем, кто отправил запрос, то вернуть список его заказов
                if (userAuth != string.Empty && userAuth == user)
                {
                    var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
                    var client = mPublishEndpoint.CreateRequestClient<UserOrdersRequest>(serviceAddress);


                    var response = await client.GetResponse<UserOrdersRespond>(new UserOrdersRequest() { user = user });

                    return new JsonResult(response.Message.userOrders);
                }
            }
            catch (Exception ex)
            {

            }


            return null;
        }
    }
}

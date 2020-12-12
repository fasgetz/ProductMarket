using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests.Discounts.Admin.Requests;
using ProductMarketModels.MassTransit.Requests.Discounts.Admin.Responds;
using ProductMarketModels.ViewModels.Admin.DiscountController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMarketApi.Controllers.Admin
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policies.Admin)]
    public class DiscountController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;

        public DiscountController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }

        [HttpPost("AddDiscount")]
        public async Task<IActionResult> disAdd([FromForm] AddDiscountViewModel discount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            await mPublishEndpoint.Publish(discount);
            return Ok("Success");
        }



        [HttpGet("RemoveDiscount")]
        public async Task<bool> disRemove(int id)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsAdminQueue");
            var client = mPublishEndpoint.CreateRequestClient<IdDiscountRequest>(serviceAddress);


            var response = await client.GetResponse<RemovedDiscountRespond>(new IdDiscountRequest() { IdDiscount = id });

            return response.Message.HasRemoved;
        }

        [HttpPost("EditDiscount")]
        public async Task<IActionResult> disEdit([FromForm] EditDiscountViewModel discount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            await mPublishEndpoint.Publish(discount);
            return Ok("Success");
        }

        /// <summary>
        /// Метод получения акций продукта
        /// </summary>
        /// <param name="idProduct">Айди продукта</param>
        /// <returns>Акции продукта</returns>
        [HttpGet("GetDiscountsProduct")]        
        public async Task<List<DiscountProduct>> getData(int idProduct)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/ProductsQueue");
            var client = mPublishEndpoint.CreateRequestClient<IdProductRequest>(serviceAddress);
                        

            var response = await client.GetResponse<GetDiscountsProductRespond>(new IdProductRequest() { idProduct = idProduct });

            return response.Message.data;
        }
    }
}

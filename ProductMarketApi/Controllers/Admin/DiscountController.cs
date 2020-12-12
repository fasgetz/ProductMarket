using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Identity;
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

            //Product product = new Product()
            //{
            //    Amount = prod.Amount,
            //    IdSubCategory = prod.idSubCategoryProduct,
            //    Name = prod.Name,
            //    Price = prod.price,
            //    Poster = imageData
            //};


            await mPublishEndpoint.Publish(discount);
            return Ok("Success");
        }
    }
}

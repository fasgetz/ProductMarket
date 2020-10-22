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

namespace ProductMarketApi.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBusControl mPublishEndpoint;

        public ProductsController(IBusControl publishEndpoint)
        {
            mPublishEndpoint = publishEndpoint;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductMarketModels;
using ProductMarketModelsElastic.Models;
using ProductMarketServices.ElasticSearch;

namespace ElasticSearchService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElasticSearchController : ControllerBase
    {


        private readonly ILogger<ElasticSearchController> _logger;
        private readonly IElasticSearchService service;

        public ElasticSearchController(ILogger<ElasticSearchController> logger, IElasticSearchService service)
        {
            _logger = logger;
            this.service = service;
        }


        [HttpGet]
        public async Task<List<ProductSuggest>> Get(string text)
        {
           if (text == null)
                return null;

            var products = await service.GetProductSuggests(text);


            return products;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(Product product)
        {

            await service.SaveSingleAsync(product);

            return Ok("success");
        }


        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {

            await service.UpdateProduct(product);

            return Ok("success");
        }
    }
}

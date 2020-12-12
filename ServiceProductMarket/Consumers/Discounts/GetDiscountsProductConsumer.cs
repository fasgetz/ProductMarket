using MassTransit;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Products.Requests;
using ProductMarketModels.MassTransit.Requests.Discounts.Admin.Responds;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.ProductsDiscount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.Discounts
{

    /// <summary>
    /// Получение акций продукта
    /// </summary>
    public class GetDiscountsProductConsumer : IConsumer<IdProductRequest>
    {
        private readonly IProductDiscountService service;

        public GetDiscountsProductConsumer(IProductDiscountService service)
        {
            this.service = service;
        }

        public async Task Consume(ConsumeContext<IdProductRequest> context)
        {
            var list = await service.GetDiscountsProduct(context.Message.idProduct);


            await context.RespondAsync<GetDiscountsProductRespond>(new GetDiscountsProductRespond() { data = list });
        }
    }
}

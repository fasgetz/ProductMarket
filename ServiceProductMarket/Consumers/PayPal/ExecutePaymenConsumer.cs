using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using PayPal.Api;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.PayPal;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketServices.Basket;
using ProductMarketServices.PayPal;
using ServiceProductMarket.Models.PayPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Consumers.PayPal
{
    public class ExecutePaymenConsumer : IConsumer<ExecutePaymentRequest>
    {
        private readonly IPayPalService PayPalService;
        private readonly IMemoryCache cache;
        private readonly IBasketService basketService;

        public ExecutePaymenConsumer(IMemoryCache cache, IPayPalService PayPalService, IBasketService basketService)
        {
            this.cache = cache;
            this.PayPalService = PayPalService;
            this.basketService = basketService;
        }


        public async Task Consume(ConsumeContext<ExecutePaymentRequest> context)
        {
            BasketPayPalModel model = cache.Get<BasketPayPalModel>(context.Message.PaymentID);

            bool success = PayPalService.ExecutePayment(context.Message.PaymentID, context.Message.PayerID, model.transactions);

            // Если оплата прошла успешно, то внести данные в БД
            if (success == true)
            {
                try
                {
                    // Товары в корзине
                    var itemsBasket = model.transactions.Select(i => i.item_list.items).FirstOrDefault().ToList();

                    var totalPrice = itemsBasket.Sum(i => Convert.ToInt32(i.quantity) * Convert.ToDecimal(i.price));

                    //if (!totalPrice.ToString().Contains('.'))
                    //{
                    //    string price = totalPrice.ToString();


                    //    price = price.Substring(0, price.Length - 2) + "." + price.Substring(price.Length - 2, 2);

                    //    totalPrice = Convert.ToDecimal(price);
                    //}

                    // Формируем заказ
                    ProductMarketModels.Order order = new ProductMarketModels.Order()
                    {
                        PayerID = context.Message.PayerID,
                        PaymentId = context.Message.PaymentID,
                        token = context.Message.Token,
                        // Сумма заказа
                        TotalPrice = totalPrice,
                        UserId = model.userName,
                        //Address = orderBasket.address,
                        Commentary = model.commentary,
                        DateOrder = DateTime.Now.AddHours(3),
                        OrderStatus = new List<OrderStatus>() // Статус заказа
                        {
                            new OrderStatus()
                            {
                                IdStatus = 1,
                                Date = DateTime.Now
                            }
                        },
                        ProductsInOrder = itemsBasket.Select(i => new ProductsInOrder() { IdProduct = Convert.ToInt32(i.sku), Count = (short)Convert.ToInt16(i.quantity), priceBuy = Convert.ToDecimal(i.price)}).ToList()
                    };

                    var addedOrder = await basketService.AddOrder(order);
                }
                catch (Exception ex)
                {
                    var message = "abc";
                }
                
            }

            await context.RespondAsync<ExecutePaymentRespond>(new ExecutePaymentRespond() { successPayment = success });
        }
    }
}

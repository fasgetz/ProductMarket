using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using PayPal.Api;
using ProductMarketModels.MassTransit.Requests.PayPal;
using ProductMarketModels.MassTransit.Responds.PayPal;
using ProductMarketServices.PayPal;
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

        public ExecutePaymenConsumer(IMemoryCache cache, IPayPalService PayPalService)
        {
            this.cache = cache;
            this.PayPalService = PayPalService;
        }


        public async Task Consume(ConsumeContext<ExecutePaymentRequest> context)
        {
            List<Transaction> transactions = cache.Get<List<Transaction>>(context.Message.PaymentID);

            bool success = PayPalService.ExecutePayment(context.Message.PaymentID, context.Message.PayerID, transactions);

            // Если оплата прошла успешно, то внести данные в БД
            if (success == true)
            {

            }

            await context.RespondAsync<ExecutePaymentRespond>(new ExecutePaymentRespond() { successPayment = success });
        }
    }
}

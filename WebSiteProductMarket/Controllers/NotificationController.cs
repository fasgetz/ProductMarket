using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {

        public NotificationController(IConfiguration config)
        {
            this.config = config;
        }

        private readonly IConfiguration config;


        [HttpPost("notification")]
        public async Task<IActionResult> StripeNotification()
        {
            System.IO.File.AppendAllText("file.txt", $"{DateTime.Now}) вызван content" + Environment.NewLine);

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                Event stripeEvent = null;

                try
                {
                    stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], config.GetValue<string>("stripeEndpointSecretNotification"));
                }
                catch(Exception message)
                {
                    return Ok(message.Message);
                }
                

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentAmountCapturableUpdated)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.PaymentIntentCanceled)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.PaymentIntentCreated)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;


                    


                    System.IO.File.AppendAllText(@"file.txt", $"{DateTime.Now}) Создан {paymentIntent.Id} | {paymentIntent.SourceId}" + Environment.NewLine);
                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.PaymentIntentProcessing)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.PaymentIntentRequiresAction)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    var handler = new HttpClientHandler();
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };

                    // Отправляем проверить платеж
                    using (var MyClient = new HttpClient(handler))
                    {
                        var payment = new
                        {
                            session_id = paymentIntent.Id
                        };


                        MyClient.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
                        var data = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                        var content = new StringContent(
                            data, Encoding.UTF8, "application/json");

                        var MyResponse = await MyClient.PostAsync("Basket/StripeExecute", content);

                        //  Если успешно прошла оплата, то вернуть данные об этом
                        if (MyResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            return BadRequest("Ошибка на стороне Stripe");

                        }
                        else
                        {
                            // Уведомить всех на Email об успешном платеже с таким-то айди на такую-то сумму от такого-то юзверя
                            System.IO.File.AppendAllText(@"file.txt", $"{DateTime.Now}) Платеж добавлен в бд {paymentIntent.Id} | {paymentIntent.Amount} - {paymentIntent.Created} | {paymentIntent.SourceId}" + Environment.NewLine);
                        }

                    }

                    System.IO.File.AppendAllText(@"file.txt", $"{DateTime.Now}) Оплачен {paymentIntent.Id} | {paymentIntent.Amount} - {paymentIntent.Created} | {paymentIntent.SourceId}" + Environment.NewLine);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                System.IO.File.AppendAllText(@"file.txt", $"{DateTime.Now}) платеж ошибка {e.Message}" + Environment.NewLine);
                return BadRequest("Тест еррор");
            }
        }





        [HttpGet("tested")]
        public IActionResult test()
        {
            return Ok($"{DateTime.Now}");
        }

    }
}

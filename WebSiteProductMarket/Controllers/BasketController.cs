using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Basket;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebSiteProductMarket.Controllers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static T GetObjects<T>(this ISession session, string key)
        {
            var value = session.Get(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }



 

    public class BasketController : Controller
    {

        private readonly IConfiguration config;

        public BasketController(IConfiguration config)
        {
            this.config = config;
        }





        /// <summary>
        /// Форма оплаты
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SuccessStripe(string session_id)
        {
            // Отправить на сервер апи со стороны этого сервера данные платежа и получить boolean успешной оплаты или нет
            try
            {


                // Очищаем кэш, т.к. оплата успешно произведена
                ClearBasket();

                ViewBag.paymentId = session_id;

                // Иначе оплата прошла успешно
                return View("Success");



                //var handler = new HttpClientHandler();
                //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                //handler.ServerCertificateCustomValidationCallback =
                //    (httpRequestMessage, cert, cetChain, policyErrors) =>
                //    {
                //        return true;
                //    };

                //// Отправляем проверить платеж
                //using (var MyClient = new HttpClient(handler))
                //{
                //    var payment = new
                //    {
                //        session_id = session_id
                //    };


                //    MyClient.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
                //    var data = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                //    var content = new StringContent(
                //        data, Encoding.UTF8, "application/json");

                //    var MyResponse = await MyClient.PostAsync("Basket/StripeExecute", content);

                //    //  Если успешно прошла оплата, то вернуть данные об этом
                //    if (MyResponse.StatusCode != System.Net.HttpStatusCode.OK)
                //    {
                //        return BadRequest("Ошибка на стороне Stripe");

                //    }

                //    // Очищаем кэш, т.к. оплата успешно произведена
                //    ClearBasket();

                //    ViewBag.paymentId = session_id;

                //    // Иначе оплата прошла успешно
                //    return View("Success");
                //}
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка на стороне Stripe");
            }

        }

        /// <summary>
        /// Форма оплаты
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Success(string PayerID, string paymentId, string token)
        {
            // Отправить на сервер апи со стороны этого сервера данные платежа и получить boolean успешной оплаты или нет
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };


                // В отдельном потоке добавить в эластик серч продукт
                using (var MyClient = new HttpClient(handler))
                {
                    var payment = new
                    {
                        PayerID = PayerID,
                        paymentId = paymentId,
                        token = token
                    };


                    MyClient.BaseAddress = new Uri(config.GetValue<string>("apiUrl"));
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(payment);
                    var content = new StringContent(
                        data, Encoding.UTF8, "application/json");

                    var MyResponse = await MyClient.PostAsync("Basket/PayPalExecute", content);

                    //  Если успешно прошла оплата, то вернуть данные об этом
                    if (MyResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return BadRequest("Ошибка на стороне PayPal");
                        
                    }

                    // Очищаем кэш, т.к. оплата успешно произведена
                    ClearBasket();

                    ViewBag.paymentId = paymentId;

                    // Иначе оплата прошла успешно
                    return View();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка на стороне PayPal");
            }

        }



        /// <summary>
        /// Форма корзины
        /// </summary>
        /// <returns></returns>
        public IActionResult Payment(string currency = "EUR")
        {
            var currencies = new List<string>() { "EUR", "USD", "BGN", "RUB" };

            if (currencies.Contains(currency))
            {
                return View("Payment", currency);
            }

            return BadRequest("Выберите действующую валюту");
        }
        
        /// <summary>
        /// Форма оплаты
        /// </summary>
        /// <returns></returns>
        public IActionResult pay()
        {
            return View();
        }


        #region Вспомогательные методы работы с корзиной

        [HttpPost]
        public IActionResult UpdateCountProduct([FromBody] ProductBasket data)
        {
            var bas = GetCart();

            // Если итем есть в корзине, то обновить данные
            bas.products.FirstOrDefault(i => i.id == data.id).count = data.count;

            // Сохраняем
            SetCart(bas);


            return Ok("success");
        }


        [HttpPost]
        public IActionResult RemoveItemBasket([FromBody] ProductBasket data)
        {
            var bas = GetCart();

            var product = bas.products.FirstOrDefault(i => i.id == data.id);

            if (product != null)
            {
                bas.products.Remove(product);
                SetCart(bas);
            }


            return Ok("success");
        }

        public IActionResult Add(ProductBasket product)
        {
            var bas = GetCart();


            if (product.id != 0)
            {
                bas.products.Add(product);
                SetCart(bas);
            }


            return Json(bas);
        }


        /// <summary>
        /// Удаление из корзины
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public bool Remove(int idProduct)
        {
            var bas = GetCart();

            var product = bas.products.FirstOrDefault(i => i.id == idProduct);

            if (product != null)
            {
                bas.products.Remove(product);
                SetCart(bas);

                return true;
            }


            return false;
        }


        public void ClearBasket()
        {
            HttpContext.Session.Clear();
        }


        /// <summary>
        /// Метод проверки, есть ли продукт в корзине
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>True, если продукт есть в базе</returns>
        public IActionResult haveItem(int idProduct)
        {
            var bas = GetCart();

            // Если итем есть в корзине, то верни true
            var hasItem = bas.products.FirstOrDefault(i => i.id == idProduct); // != null ? true : false;

            return Json(hasItem);
        }

        public Basket GetCart()
        {
            var bas = HttpContext.Session.GetObjects<Basket>("basket") ?? new Basket();            

            return bas;
        }

        public IActionResult GetCartJson()
        {
            return Json(GetCart());
        }

        public void SetCart(Basket basket)
        {
            HttpContext.Session.SetObject<Basket>("basket", basket);
        }


        #endregion
    }


}

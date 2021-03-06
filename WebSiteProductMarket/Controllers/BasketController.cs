﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public BasketController()
        {

        }







        /// <summary>
        /// Форма корзины
        /// </summary>
        /// <returns></returns>
        public IActionResult Payment()
        {
            return View();
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

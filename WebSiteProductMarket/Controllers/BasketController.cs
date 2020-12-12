using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
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


    #region Сущности корзины

    public class Product
    {
        public int id { get; set; }

        // Количество
        public int count { get; set; }
    }

    public class Basket
    {
        public List<Product> products { get; set; } = new List<Product>();
        //public int count { get; set; }
    }

    #endregion


    public class BasketController : Controller
    {
        public BasketController()
        {

        }



        #region Вспомогательные методы работы с корзиной

        public IActionResult Add(Product product)
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

        /// <summary>
        /// Метод проверки, есть ли продукт в корзине
        /// </summary>
        /// <param name="idProduct">Номер продукта</param>
        /// <returns>True, если продукт есть в базе</returns>
        public bool haveItem(int idProduct)
        {
            var bas = GetCart();

            // Если итем есть в корзине, то верни true
            var hasItem = bas.products.FirstOrDefault(i => i.id == idProduct) != null ? true : false;

            return hasItem;
        }

        public Basket GetCart()
        {
            var bas = HttpContext.Session.GetObjects<Basket>("basket") ?? new Basket();

            return bas;
        }

        public void SetCart(Basket basket)
        {
            HttpContext.Session.SetObject<Basket>("basket", basket);
        }


        #endregion
    }


}

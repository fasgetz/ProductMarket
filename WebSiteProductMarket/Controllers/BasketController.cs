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


    public class Product
    {
        public int id { get; set; }

        // Количество
        public int count { get; set; }
    }
    public class Basket
    {
        public List<Product> products { get; set; } = new List<Product>();
        public int count { get; set; }
    }


    public class BasketController : Controller
    {
        public BasketController()
        {

        }


        public IActionResult Add(Product product)
        {
            var bas = GetCart();
            bas.count++;

            bas.products.Add(product);
            SetCart(bas);

            return Json(bas);

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
    }


}

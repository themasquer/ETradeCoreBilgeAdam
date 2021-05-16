using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult AddToCart(int? productId)
        {
            if (productId == null)
                return View("NotFound");
            var product = _productService.Query().SingleOrDefault(p => p.Id == productId.Value);
            string userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            List<CartModel> cart = new List<CartModel>();
            CartModel cartItem;
            string cartJson;

            // .NET Framework web uygulamaları (MVC ve Web Froms): session'a her tipten obje atılabilir
            //cart = Session["cart"] as List<CartModel>; // session'dan veri alma
            //Session["cart"] = cart; // session'a veri atma
            //Session.Remove("cart"); // session'daki veriyi temizleme

            // .NET Core web uygulamaları (ASP.NET Core MVC): session'a sadece int?, string veya byte[] tiplerinde veri atılabilir
            if (HttpContext.Session.GetString("cart") != null)
            {
                cartJson = HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<List<CartModel>>(cartJson);
            }
            cartItem = new CartModel()
            {
                ProductId = productId.Value,
                UserId = Convert.ToInt32(userId),
                ProductName = product.Name,
                UnitPrice = product.UnitPrice
            };
            cart.Add(cartItem);
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartJson);

            //TempData["ProductsMessage"] = product.Name + " added to cart.";
            //return RedirectToAction("Index", "Products");

            // genelde mesajlar (message) route üzerinden gönderilmez, TempData, ViewData veya ViewBag üzerinden gönderilir!
            return RedirectToAction("Index", "Products", new { message = product.Name + " added to cart.", id = product.Id });
        }

        public IActionResult Index()
        {
            List<CartModel> cart = new List<CartModel>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
            }

            // group by sadece List için değil IQueryable için de kullanılabilir!
            //List<CartGroupByModel> cartGroupBy = (from c in cart
            //    //group c by c.ProductName // tek bir özellik gruplanıyorsa kullanılır
            //    group c by new { c.ProductId, c.UserId, c.ProductName } // birden çok özellik gruplanıyorsa kullanılır
            //    into cGroupBy
            //    select new CartGroupByModel()
            //    {
            //        ProductId = cGroupBy.Key.ProductId,
            //        UserId = cGroupBy.Key.UserId,
            //        ProductName = cGroupBy.Key.ProductName,
            //        TotalUnitPriceText = "$" + cGroupBy.Sum(cgb => cgb.UnitPrice).ToString(new CultureInfo("en")),
            //        TotalCount = cGroupBy.Count()
            //    }).ToList();
            List<CartGroupByModel> cartGroupBy = cart.GroupBy(
                //c => c.ProductName
                c => new { c.ProductId, c.UserId, c.ProductName }
                ).Select(cGroupBy => new CartGroupByModel()
                {
                    ProductId = cGroupBy.Key.ProductId,
                    UserId = cGroupBy.Key.UserId,
                    ProductName = cGroupBy.Key.ProductName,
                    TotalUnitPriceText = "$" + cGroupBy.Sum(cgb => cgb.UnitPrice).ToString(new CultureInfo("en")),
                    TotalCount = cGroupBy.Count()
                }).ToList();
            cartGroupBy = cartGroupBy.OrderBy(cgb => cgb.ProductName).ToList();

            return View(cartGroupBy);
        }

        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("cart");
            TempData["CartMessage"] = "Cart cleared";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int? productId, int? userId)
        {
            if (productId == null || userId == null)
                return View("NotFound");
            CartModel item = null;
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<CartModel> cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
                item = cart.FirstOrDefault(c => c.ProductId == productId.Value && c.UserId == userId.Value);
                if (item != null)
                    cart.Remove(item);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            if (item != null)
                TempData["CartMessage"] = item.ProductName + " removed from cart";
            return RedirectToAction(nameof(Index));
        }
    }
}

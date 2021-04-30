using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopifyMVC.Data;
using ShopifyMVC.Models;
using ShopifyMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ApplicationDbContext _db;

        public CartController(ILogger<CartController> logger, ApplicationDbContext DB)
        {
            _logger = logger;
            _db = DB;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> newCartList = new List<ShoppingCart>();

            //Before Adding- get the initial Values in the Session
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey).Count() > 0 &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey) != null)
            {
                newCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionKey);
            }

            var cartListProductIds = newCartList.Select(n => n.ProductId);

            //Another way of writing Nested Loop to compare two Lists
            var cartProductList = _db.Products.Where(u => cartListProductIds.Contains(u.Id)).ToList();

            return View(cartProductList);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> newCartList = new List<ShoppingCart>();

            //Before Adding- get the initial Values in the Session
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey).Count() > 0 &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey) != null)
            {
                newCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionKey);
            }

           
            //Aftergetting the initial Valuein the session- add the new ProductID
            newCartList.Add(new ShoppingCart { ProductId = id });

            //Set the New Session Dictionary  with the new Cart List
            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WebConstant.SessionKey, newCartList);


            return RedirectToAction("Index", "Home");
        }


        public IActionResult RemoveFromCart(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> previousCartList = new List<ShoppingCart>();

            //Before Adding- get the initial Values in the Session
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey).Count() > 0 &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey) != null)
            {
                previousCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionKey);
            }

            List<ShoppingCart> newCartList = new List<ShoppingCart>();


            //After getting the all Values in the session, iterate throught the list and check
            foreach (var item in previousCartList)
            {
                if(item.ProductId != id)
                {
                    newCartList.Add(item);
                }
            }

            //Set the New Session Dictionary  with the new Cart List
            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WebConstant.SessionKey, newCartList);


            return RedirectToAction("Index", "Home");
        }

    }
}

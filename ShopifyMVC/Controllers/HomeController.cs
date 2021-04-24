using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopifyMVC.Data;
using ShopifyMVC.Models;
using ShopifyMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext DB)
        {
            _logger = logger;
            _db = DB;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Categories = _db.Category.ToList(),
                Products = _db.Products.Include(u=>u.Category).ToList()

            };
            return View(homeVM);
        }


        /**
         * Anytime a Product in the Shop is clicked, we are to help the user display the product details
         * We get the product details from the Database and display it in the the view. 
         * However it is possible that the user already havethe product in the Cart, so we have add to change
         * the button from add to cart to Remove from the Cart
         *  
         */
        public IActionResult ProductDetails(int? id)
        {

            if (id == null || id== 0)
            {
                return NotFound();
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Products.Include(p => p.Category).FirstOrDefault(u => u.Id == id),
                ExistInCart = false
            };
                
            List<ShoppingCart> cartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionKey);
            }

            //Exact the ProductIds from the List inside the cart
            //1. Iterate through the List and check if any ProductId == id of the Specific Product(Parameter Id)
            //2. if yes then change the ExistINCart to true

            foreach(var item in cartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistInCart = true;
                }
            }


            return View(detailsVM);
        }


        [HttpPost]
        public IActionResult ProductDetails(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> newCartList = new List<ShoppingCart>();

            //Before Adding- get the initial Values in the Session
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionKey).Count() > 0)     
            {
                newCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionKey);
            }
            //Aftergetting the initial Valuein the session- add the new ProductID
            newCartList.Add(new ShoppingCart { ProductId = id });

            //Set the New Session Dictionary  with the new Cart List
            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WebConstant.SessionKey, newCartList);


            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
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
                Products = _db.Products.ToList()

            };
            return View(homeVM);
        }

        public IActionResult ProductDetails(int? id)
        {

            if (id == null || id== 0)
            {
                return NotFound();
            }

            DetailsVM detailsVM = new DetailsVM();

            var product = _db.Products.Find(id);

            detailsVM.Product = product;

            return View(detailsVM);
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

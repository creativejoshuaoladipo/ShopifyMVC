using Microsoft.AspNetCore.Mvc;
using ShopifyMVC.Data;
using ShopifyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var categoryList = _db.Category.ToList();
            return View(categoryList);
        }

        public IActionResult AddCategory()
        {
            var category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult AddCategory(Category objVM)
        {
            if (!ModelState.IsValid)
            {
                return View(objVM);
            }

            _db.Category.Add(objVM);
            _db.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult EditCategory(int? Id)
        {

            if (Id == null|| Id == 0)
            {
                return NotFound();
            }

          var category =  _db.Category.Where(c => c.Id == Id).FirstOrDefault();

            return View(category);
        }


        [HttpPost]
        public IActionResult EditCategory(Category objVM)
        {

            if (!ModelState.IsValid)
            {
                return View(objVM);
            }

            _db.Category.Update(objVM);
            _db.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult DeleteCategory(int? Id)
        {

            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var category = _db.Category.Where(c => c.Id == Id).FirstOrDefault();

            return View(category);
        }


        [HttpPost]
        public IActionResult DeleteCategory(Category objVM)
        {

            if (!ModelState.IsValid)
            {
                return View(objVM);
            }

            _db.Category.Remove(objVM);
            _db.SaveChanges();

            return RedirectToAction("index");
        }


    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopifyMVC.Data;
using ShopifyMVC.Models;
using ShopifyMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        //Create a Table
        public IActionResult Index()
        {
            /*When deleting a product, it is save we don't totally remove the product data from the DB
            //We will need to introduce a boolean column- isDelete == true to help seperate the list of product that
            //should be posted on the view
            */
           var productList= _db.Products.Include("Category").Where(p=> p.IsDelete == true).ToList();
            return View(productList);
        }

        //Create a Product Form to add Product to the DB
        public IActionResult AddProduct()
        {
            //wE NEED THE LIST OF CATEGORY TO GET NAME AND 
            //THE ID OF THE CATEGORY THAT WILL BE NEEDED FOR THE DROPDOWNLIST
            var categories = _db.Category.ToList();

            ProductVM product = new ProductVM()
            {
                Product = new Product(),

                CategoryList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };
            return View(product);
        }


        //Post a Product Form to the DB
        //Added the IFormFile because the picture that will be uploaded on the server 

        [HttpPost(Name ="AddProduct")]
        public IActionResult AddProductPost(Product obj, IFormFile file)
        {

          if(!ModelState.IsValid)
            {
                return View(obj);

            }

            /*Since the form will also be posting a file then we have to get the file path
            We created a folder(ProductImages) inside the wwwroot folder
            We use IWebHostEnvironment to get the WebRootPath- wwwroot..
            Thereafter we will concatenate it with the folder name
            */

            string filePath = _host.WebRootPath + "ProductImages";

            //A complete PathName is filePath + ImageName
            //To create an Image Name for the Picture uploaded-
            // Guid + PathExtension----.jpeg,png( IFormFile.FileName

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string fullFilePath = Path.Combine(filePath, fileName);


            //Now that we have gotten the file path- we will create a FileStream
            // that can read the File or store the Picture file for us ins
            // we can use file.CopyTo() -- to copy the file to the file stream Object creaded

            using (var stream = new FileStream(fullFilePath , FileMode.Create))
            {
                file.CopyTo(stream);
            }

            //After we had successfully copied the file into the stream,
            //we should remember to save the fileName to the DB as shown below
            obj.ProductImage = fileName;
            _db.Products.Add(obj);

            _db.SaveChanges();
            

            return RedirectToAction("Index"); 

        }

        //Print out an existing Form that will be edited
        public IActionResult EditProduct(int productId)
        {
            var product = _db.Products.Find(productId);

            if(product== null)
            {
                throw new ArgumentNullException("The Id cannot be found");
            }

            ProductVM obj = new ProductVM()
            {
                Product = product,
                CategoryList = _db.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };


            return View(obj);
        }

        [HttpPost(Name = "EditProduct")]
        public IActionResult EditProductPost(Product obj, IFormFile file)
        {

            if (!ModelState.IsValid)
            {
                return View(obj);

            }
            //When Updating the file, check whether there is a file uploaded.
            //The only way to know if by checking if the file length is greater than 0(if(file.Length >0))
            //If yes, then the user uploaded a set of data you may need to save by creating a new FileStream Object
            // Else remember to save the DatabaseImageName as the new image Name before Updating the obj
            // or you simply say... if file == null ---use DBNAME
            if (file.Length > 0)
            {


                //Since the form will also be posting a file then we have to get the file path
                //We created a folder(ProductImages) inside the wwwroot folder
                //We use IWebHostEnvironment to get the WebRootPath- wwwroot..
                //Thereafter we will concatenate it with the folder name
                string filePath = _host.WebRootPath + "ProductImages";

                //A complete PathName is filePath + ImageName
                //To create an Image Name for the Picture uploaded-
                // Guid + PathExtension----.jpeg,png( IFormFile.FileName

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                string fullFilePath = Path.Combine(filePath, fileName);


                //Now that we have gotten the file path- we will create a FileStream
                // that can read the File or store the Picture file for us ins
                // we can use file.CopyTo() -- to copy the file to the file stream Object creaded

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                //After we had successfully copied the file into the stream,
                //we should remember to save the fileName to the DB as shown below
                obj.ProductImage = fileName;
            }
            else 
            {
                // Use the DB data to save the image Name 
                var dbProduct = _db.Products.Where(p => p.Id == obj.Id).FirstOrDefault();
                obj.ProductImage = dbProduct.ProductImage;
            }

            _db.Products.Update(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

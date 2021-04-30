using AutoMapper;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
       private readonly IMapper _map;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment host, IMapper map)
        {
            _db = db;
            _webHostEnvironment = host;
            _map = map;

        }

        //Create a Table
        public IActionResult Index()
        {
            /*When deleting a product, it is save we don't totally remove the product data from the DB
            //We will need to introduce a boolean column- isDelete == true to help seperate the list of product that
            //should be posted on the view
            */
           var productList= _db.Products.Include("Category").Where(p=> p.IsDelete == false).ToList();

            //Since we want to map a domain object List to a DTOList we will create the DTOList
            //var productListVM = new List<ProductVM>();

            //foreach(var product in productList)
            //{
            //    //productListVM.Add(_map.Map<ProductVM>(product));
            //}

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

        [HttpPost]
        public IActionResult AddProduct(ProductVM objVM, IFormFile file)
        {

          if(!ModelState.IsValid)
            {
                return View(objVM);

            }


            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;


            //Creating a file path that the image will be saved
            string upload = webRootPath + @"\ProductImages\";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            objVM.Product.ProductImage = fileName + extension;


            //Remember to set Created DateTime to the Current Date
            objVM.Product.CreatedDate = DateTime.Now;

            //Remember to set is deletedto false so that it will show on the table
            objVM.Product.IsDelete = false;

            //Check it latter to know why you couldn't map it
            //Before Adding the Product to the Database- map it from the View Model to the Domain Class
          //  var product = _map.Map<ProductVM, Product>(obj);

            _db.Products.Add(objVM.Product);

            _db.SaveChanges();
            

            return RedirectToAction("Index"); 

        }

        //Print out an existing Form that will be edited
        public IActionResult EditProduct(int id)
        {
            var product = _db.Products.Find(id);

            if(product== null)
            {
                throw new ArgumentNullException("The Product cannot be found");
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

        [HttpPost]
        public IActionResult EditProduct(ProductVM objVM, IFormFile file)
        {

            if (!ModelState.IsValid)
            {
                return View(objVM);

            }

            // Use the DB data to save the image Name 
            var dbProduct = _db.Products.Where(p => p.Id == objVM.Product.Id).AsNoTracking().FirstOrDefault();

            //When Updating the file, check whether there is a file uploaded.
            //The only way to know if by checking if the file length is greater than 0(if(file.Length >0))
            //If yes, then the user uploaded a set of data you may need to save by creating a new FileStream Object
            // Else remember to save the DatabaseImageName as the new image Name before Updating the obj
            // or you simply say... if file == null ---use DBNAME


            var files = HttpContext.Request.Form.Files;
            

            if (files.Count > 0)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;


                //Creating a file path that the image will be saved
                string upload = webRootPath + @"\ProductImages\";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                var oldFile = Path.Combine(upload, dbProduct.ProductImage);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                objVM.Product.ProductImage = fileName + extension;


                ////Since the form will also be posting a file then we have to get the file path
                ////We created a folder(ProductImages) inside the wwwroot folder
                ////We use IWebHostEnvironment to get the WebRootPath- wwwroot..
                ////Thereafter we will concatenate it with the folder name
                //string filePath = _webHostEnvironment.WebRootPath + "ProductImages";

                ////A complete PathName is filePath + ImageName
                ////To create an Image Name for the Picture uploaded-
                //// Guid + PathExtension----.jpeg,png( IFormFile.FileName

                //string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                //string fullFilePath = Path.Combine(filePath, fileName);


                ////Now that we have gotten the file path- we will create a FileStream
                //// that can read the File or store the Picture file for us ins
                //// we can use file.CopyTo() -- to copy the file to the file stream Object creaded

                //using (var stream = new FileStream(fullFilePath, FileMode.Create))
                //{
                //    file.CopyTo(stream);
                //}

                ////After we had successfully copied the file into the stream,
                ////we should remember to save the fileName to the DB as shown below
                //obj.Product.ProductImage = fileName;
            }
            else 
            {
              
               
                objVM.Product.ProductImage = dbProduct.ProductImage;
            }
            //Created Date- will remain the same as before-DB result
            objVM.Product.CreatedDate = dbProduct.CreatedDate;
            //Modified Date
            objVM.Product.ModifiedDate = DateTime.Now;

            //Remember to set is deletedto false so that it will show on the table
            objVM.Product.IsDelete = false;
            //Before Adding it to the Database map itfrom the View Model to the Domain Class
            //var product = _map.Map<Product>(obj);
            _db.Products.Update(objVM.Product);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult DeleteProduct(int? id)
        {
            var product = _db.Products.Find(id);

            if(product == null)
            {
                throw new ArgumentNullException("No Product with this Id can be found");
            }

            var catList = _db.Category.ToList();
            ProductVM prodVM = new ProductVM
            {
                Product = product,
                CategoryList = catList.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }

                    )
            };
            return View(prodVM);
        }


        [HttpPost]
        [ActionName("DeleteProduct")]
        public IActionResult DeleteProductPost(int? id)
        {

            var DbProduct = _db.Products.Find(id);

            var webRootPath = _webHostEnvironment.WebRootPath;

            var imageFolderPath = webRootPath + @"\ProductImages\";

            var imageLocation = Path.Combine(imageFolderPath, DbProduct.ProductImage);


            if (System.IO.File.Exists(imageLocation))
            {
                System.IO.File.Delete(imageLocation);
            }


            _db.Products.Remove(DbProduct);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
       
        [HttpGet]

        public IActionResult Detail(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }

            ProductDetailVM pvm = new ProductDetailVM();

            var dbProduct = _db.Products.Where(p => p.Id == id).AsNoTracking().FirstOrDefault();
            if(dbProduct != null)
            {
                pvm.Product = dbProduct;

                return View("Detail",pvm);
            }

            return NotFound();



        }



    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RockyProject.Data;
using RockyProject.Models;
using RockyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RockyProject.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
       
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            // var products = _db.Product.Include(i => i.Category).ToList();
            IEnumerable<Product> objList = _db.Products;
            foreach (var item in objList)
            {
                item.Category = _db.Category.FirstOrDefault(u => u.Id == item.CategoryId);
            }
            return View(objList);
        }


        //GET Upsert Action
        public IActionResult UpSert(int? id)
        {
            // Product product = new Product();

            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //}) ;
            ////ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };


            if (id == null)
            {
                //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
                //{
                //    Text = i.Name,
                //    Value = i.Id.ToString()
                //}) ;
                ////ViewBag.CategoryDropDown = CategoryDropDown;
                //ViewData["CategoryDropDown"] = CategoryDropDown;


                return View(productVm);
            }
            else
            {
                productVm.Product = _db.Products.Find(id);
                if (productVm.Product == null)
                {
                    return NotFound();

                }
                return View(productVm);
            }

        }



        //Post Upsert Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _hostEnvironment.WebRootPath;
                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extention = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = filename + extention;
                    productVM.Product.ApplicationTypeId = 2;
                    _db.Products.Add(productVM.Product);


                }
                else
                {
                    //Updating
                    var ObjFromDb = _db.Products.AsNoTracking().FirstOrDefault(c => c.Id == productVM.Product.Id);

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extention = Path.GetExtension(files[0].FileName);

                        var oldfile = Path.Combine(upload, ObjFromDb.Image);
                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = filename + extention;
                    }
                    else
                    {
                        productVM.Product.Image = ObjFromDb.Image;
                    }
                    _db.Products.Update(productVM.Product);

                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(productVM);
            }
        }


        //GET Delete Action
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            else
            {
                var product = _db.Products.FirstOrDefault(c => c.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    string webRootPath = _hostEnvironment.WebRootPath;
                    string upload = webRootPath + WC.ImagePath;
                    var file = Path.Combine(upload, product.Image);
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

        }


        ////Post Edit Action
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete(int id)
        //{
        //    var category = _db.Category.FirstOrDefault(c=>c.Id == id);
        //    _db.Category.Remove(category);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");

        //}

    }
}

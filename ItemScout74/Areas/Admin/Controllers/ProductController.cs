using ScoutDAL.Data;
using ScoutModels;
using Microsoft.AspNetCore.Mvc;
using ScoutDAL.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoutModels.ViewModels;
using System.Collections.Generic;

namespace ItemScout74.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id) //update and create
        {
            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u=>new SelectListItem //Product Managementte Categoryleri list etmek için
                {
                    Text = u.Name,
                    Value = u.Id.ToString() //db verilerini dinamik yaparken projection kullanımı
                }),
                Product = new Product()
            };

            if (id==null || id ==0) //create
            {
                return View(productVM);
            }

            else //update
            {
                productVM.Product=_unitOfWork.Product.Get(u=>u.Id==id);
                return View(productVM);
            }
            //ViewBag.CategoryList = CategoryList; //Viewbag controllerden viewe data transferi sağlıyor, viewbag's life lasts during the current http request.
            //ViewData["CategoryList"] = CategoryList; chatgpt

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file) 
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //random name to image files
                    string productPath = Path.Combine(wwwRootPath,@"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //deleting old image if we want to update image
                        var oldImagePath = Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVM.Product.Id==0)
                {
                   _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");

            }

            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem //Product Managementte Categoryleri list etmek için (dropdown)
                {
                    Text = u.Name,
                    Value = u.Id.ToString() //db verilerini dinamik yaparken projection kullanımı
                });
                return View(productVM);
            }
        }

        #region API
        [HttpGet]
        public IActionResult GetAll() //admin/product/getall
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id) //admin/product/getall
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new {success = false, message="Error while deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message="Delete Successful" });
        }
        #endregion
    }
}

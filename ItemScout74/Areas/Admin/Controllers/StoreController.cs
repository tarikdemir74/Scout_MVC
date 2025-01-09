using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoutDAL.Repository.IRepository;
using ScoutModels.ViewModels;
using ScoutModels;
using ScoutUtility;

namespace ItemScout74.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StoreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public StoreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public IActionResult Index()
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();
            return View(objStoreList);
        }

        public IActionResult Upsert(int? id) //update and create
        {
            
            if (id == null || id == 0) //create
            {
                return View(new Store());
            }

            else //update
            {
                Store storeObj = _unitOfWork.Store.Get(u => u.Id == id);
                return View(storeObj);
            }
            //ViewBag.CategoryList = CategoryList; //Viewbag controllerden viewe data transferi sağlıyor, viewbag's life lasts during the current http request.
            //ViewData["CategoryList"] = CategoryList; chatgpt

        }

        [HttpPost]
        public IActionResult Upsert(Store StoreObj)
        {

            if (ModelState.IsValid)
            {
                if (StoreObj.Id == 0)
                {
                    _unitOfWork.Store.Add(StoreObj);
                }
                else
                {
                    _unitOfWork.Store.Update(StoreObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Store created successfully";
                return RedirectToAction("Index");

            }

            else
            {
                return View(StoreObj);
            }
        }

        #region API
        [HttpGet]
        public IActionResult GetAll() //admin/Store/getall
        {
            List<Store> objStoreList = _unitOfWork.Store.GetAll().ToList();
            return Json(new { data = objStoreList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) //admin/Store/getall
        {
            var StoreToBeDeleted = _unitOfWork.Store.Get(u => u.Id == id);
            if (StoreToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Store.Remove(StoreToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}

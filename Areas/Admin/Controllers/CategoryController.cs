using ECommerce.BusinessLogic.IManagers;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantValues.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _categoryManager.Add(categoryViewModel);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id < 1)
            {
                return NotFound();
            }

            CategoryViewModel categoryViewModel = _categoryManager.Get(id.Value);
            if (categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }


        [HttpPost]
        public IActionResult Update(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                _categoryManager.Update(categoryViewModel);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        #region API Calls

        public IActionResult GetAll()
        {
            List<CategoryViewModel> categories = _categoryManager.GetAll();

            return Json(new { data = categories });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            CategoryViewModel categoryViewModel = _categoryManager.Get(id.Value);

            if (categoryViewModel == null)
            {
                return NotFound();
            }

            _categoryManager.Delete(id.Value);
            TempData["success"] = "Category deleted successfully";
            return Json(new { success = true, message = "Delete Successful" });
        } 
        #endregion
    }
}

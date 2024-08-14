using ECommerce.BusinessLogic.IManagers;
using ECommerce.BusinessLogic.Managers;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantValues.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IProductImageManager _productImageManager;

        public ProductController(IProductManager productManager,
            ICategoryManager categoryManager,
            IProductImageManager productImageManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _productImageManager = productImageManager;
        }

        public IActionResult Index()
        {
            //List<ProductViewModel> products = _productManager.GetAll();
            //return View(products);
            return View();
        }


        public IActionResult Add()
        {
            ProductViewModel productViewModel = new()
            {
                Categories = _categoryManager.GetAllActive().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ProductCategories = new List<ProductCategoryViewModel>(),
                ProductImages = new List<ProductImageViewModel>()
            };
            return View(productViewModel);
        }

        [HttpPost]
        public IActionResult Add(ProductViewModel productViewModel, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {
                int productId = _productManager.Add(productViewModel, files);
                if (productId == 0)
                {
                    return NotFound();
                }

                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productViewModel.Categories = _categoryManager.GetAllActive().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productViewModel);
            }
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id < 1)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = _productManager.Get(id.Value);
            if (productViewModel == null)
            {
                return NotFound();
            }

            productViewModel.Categories = _categoryManager.GetAllActive().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(productViewModel);
        }


        [HttpPost]
        public IActionResult Update(ProductViewModel productViewModel, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                int productId = _productManager.Update(productViewModel, files);
                if (productId == 0)
                {
                    return NotFound();
                }

                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productViewModel.Categories = _categoryManager.GetAllActive().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productViewModel);
            }
        }



        #region API Calls
        public IActionResult GetAll()
        {
            List<ProductViewModel> products = _productManager.GetAll();

            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = _productManager.Get(id.Value);

            if (productViewModel == null)
            {
                return NotFound();
            }

            _productManager.Delete(id.Value);
            TempData["success"] = "Product deleted successfully";
            return Json(new { success = true, message = "Delete Successful" });
        }



        public IActionResult DeleteImage(int imageId)
        {
            int productId = _productImageManager.Delete(imageId);
            if (productId > 0 )
            {
                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Update), new { id = productId });

        }
        #endregion
    }
}

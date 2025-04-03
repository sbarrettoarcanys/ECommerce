using System.Threading.Tasks;
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
            return View();
        }


        public async Task<IActionResult> Add()
        {
            var categories = await _categoryManager.GetAllActive();
            ProductViewModel productViewModel = new()
            {
                Categories = categories.Select(x => new SelectListItem
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
        public async Task<IActionResult> Add(ProductViewModel productViewModel, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {
                int productId = await _productManager.Add(productViewModel, files);
                if (productId == 0)
                {
                    return NotFound();
                }

                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                var categories = await _categoryManager.GetAllActive();
                productViewModel.Categories = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productViewModel);
            }
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = await _productManager.Get(id.Value);
            if (productViewModel == null)
            {
                return NotFound();
            }
            var categories = await _categoryManager.GetAllActive();
            productViewModel.Categories = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(productViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel productViewModel, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                int productId = await _productManager.Update(productViewModel, files);
                if (productId == 0)
                {
                    return NotFound();
                }

                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                var categories = await _categoryManager.GetAllActive();
                productViewModel.Categories = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productViewModel);
            }
        }



        #region API Calls
        public async Task<IActionResult> GetAll()
        {
            List<ProductViewModel> products = await _productManager.GetAll();

            return Json(new { data = products });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = await _productManager.Get(id.Value);

            if (productViewModel == null)
            {
                return NotFound();
            }

            await _productManager.Delete(id.Value);
            TempData["success"] = "Product deleted successfully";
            return Json(new { success = true, message = "Delete Successful" });
        }



        public async Task<IActionResult> DeleteImage(int imageId)
        {
            int productId = await _productImageManager.Delete(imageId);
            if (productId > 0)
            {
                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Update), new { id = productId });

        }
        #endregion
    }
}

using ECommerce.BusinessLogic.IManagers;
using ECommerce.BusinessLogic.Managers;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerce.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductManager _productManager;
        private readonly IShoppingCartManager _shoppingCartManager;

        public HomeController(ILogger<HomeController> logger,
            IShoppingCartManager shoppingCartManager,
            IProductManager productManager
            )
        {
            _logger = logger;
            _productManager = productManager;
            _shoppingCartManager = shoppingCartManager;
        }

        public IActionResult Index(string searchText)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            if (!string.IsNullOrEmpty(searchText))
            {
                products = _productManager.GetAllActive(searchText);
            }
            else
            {
                products = _productManager.GetAllActive();
            }

            IndexViewModel indexViewModel = new IndexViewModel
            {
                ProductViewModels = products,
            };

            return View(indexViewModel);
        }


        public IActionResult Details(int productId)
        {
            ShoppingCartViewModel shoppingCartViewModel = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _productManager.Get(productId),
            };
            return View(shoppingCartViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartViewModel.ApplicationUserId = userId;

            int id = _shoppingCartManager.Add(shoppingCartViewModel);
            if (id != 0)
            {
                TempData["success"] = "Cart updated successfully";
            }

            HttpContext.Session.SetInt32(ConstantValues.SessionCart, _shoppingCartManager.GetAll(userId).Count());
            return RedirectToAction(nameof(Index));
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

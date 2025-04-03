using ECommerce.BusinessLogic.IManagers;
using ECommerce.BusinessLogic.Managers;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(string searchText)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            if (!string.IsNullOrEmpty(searchText))
            {
                products = await _productManager.GetAllActive(searchText);
            }
            else
            {
                products = await _productManager.GetAllActive();
            }

            IndexViewModel indexViewModel = new IndexViewModel
            {
                ProductViewModels = products,
            };

            return View(indexViewModel);
        }


        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCartViewModel shoppingCartViewModel = new()
            {
                Count = 1,
                ProductId = productId,
                Product = await _productManager.Get(productId),
            };
            return View(shoppingCartViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCartViewModel shoppingCartViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartViewModel.ApplicationUserId = userId;

            int id = await _shoppingCartManager.Add(shoppingCartViewModel);
            if (id != 0)
            {
                TempData["success"] = "Cart updated successfully";
            }

            List<ShoppingCartViewModel> shoppingCarts = await _shoppingCartManager.GetAll(userId);

            HttpContext.Session.SetInt32(ConstantValues.SessionCart, shoppingCarts.Count());
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

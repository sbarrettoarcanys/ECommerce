using ECommerce.BusinessLogic.IManagers;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartManager _shoppingCartManager;
        private readonly IProductImageManager _productImageManager;

        public CartController(IShoppingCartManager shoppingCartManager, IProductImageManager productImageManager)
        {
            _shoppingCartManager = shoppingCartManager;
            _productImageManager = productImageManager;
        }

        public async Task<IActionResult> Index()
        {
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartDetailsViewModel = await _shoppingCartManager.GetShoppingCartDetails(userId);
            foreach (var cart in shoppingCartDetailsViewModel.ShoppingCartViewModels)
            {
                cart.Product.ProductImages = await _productImageManager.GetAll(cart.ProductId);
            }

            return View(shoppingCartDetailsViewModel);
        }

        #region API calls

        [HttpPost]
        public async Task<JsonResult> PlusJson(int cartid)
        {
            ShoppingCartViewModel shoppingCartViewModel = await _shoppingCartManager.Get(cartid);
            shoppingCartViewModel.Count += 1;
            await _shoppingCartManager.Update(shoppingCartViewModel);

            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var shoppingCartDetailsViewModel = await _shoppingCartManager.GetShoppingCartDetails(userId);

            return Json(new { shoppingCartViewModel, orderTotalString = shoppingCartDetailsViewModel.OrderTotalString });
        }
        [HttpPost]
        public async Task<IActionResult> MinusJson(int cartid)
        {
            ShoppingCartViewModel shoppingCartViewModel = await _shoppingCartManager.Get(cartid);
            shoppingCartViewModel.Count -= 1;

            if (shoppingCartViewModel.Count <= 0)
            {
                //remove that from cart
                await _shoppingCartManager.Delete(cartid);
            }
            else
            {
                await _shoppingCartManager.Update(shoppingCartViewModel);
            }

            var shoppingCarts = await _shoppingCartManager.GetAll(shoppingCartViewModel.ApplicationUserId);
            HttpContext.Session.SetInt32(ConstantValues.SessionCart,
                shoppingCarts.Count());

            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartDetailsViewModel = await _shoppingCartManager.GetShoppingCartDetails(userId);

            return Json(new { shoppingCartViewModel, orderTotalString = shoppingCartDetailsViewModel.OrderTotalString });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveJson(int cartid)
        {
            _shoppingCartManager.Delete(cartid);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCarts = await _shoppingCartManager.GetAll(claim.Value);
            HttpContext.Session.SetInt32(ConstantValues.SessionCart,
                shoppingCarts.Count());

            var shoppingCartDetailsViewModel = await _shoppingCartManager.GetShoppingCartDetails(claim.Value);

            return Json(shoppingCartDetailsViewModel);

        }
        #endregion
    }
}

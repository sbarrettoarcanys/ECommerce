using ECommerce.BusinessLogic.IManagers;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public IActionResult Index()
        {
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartDetailsViewModel = _shoppingCartManager.GetShoppingCartDetails(userId);
            foreach (var cart in shoppingCartDetailsViewModel.ShoppingCartViewModels)
            {
                cart.Product.ProductImages = _productImageManager.GetAll(cart.ProductId);
            }

            return View(shoppingCartDetailsViewModel);
        }

        #region API calls

        [HttpPost]
        public JsonResult PlusJson(int cartid)
        {
            ShoppingCartViewModel shoppingCartViewModel = _shoppingCartManager.Get(cartid);
            shoppingCartViewModel.Count += 1;
            _shoppingCartManager.Update(shoppingCartViewModel);

            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            var shoppingCartDetailsViewModel = _shoppingCartManager.GetShoppingCartDetails(userId);

            return Json(new { shoppingCartViewModel, orderTotalString = shoppingCartDetailsViewModel.OrderTotalString });
        }
        [HttpPost]
        public IActionResult MinusJson(int cartid)
        {
            ShoppingCartViewModel shoppingCartViewModel = _shoppingCartManager.Get(cartid);
            shoppingCartViewModel.Count -= 1;

            if (shoppingCartViewModel.Count <= 0)
            {
                //remove that from cart
                _shoppingCartManager.Delete(cartid);
            }
            else
            {
                _shoppingCartManager.Update(shoppingCartViewModel);
            }

            HttpContext.Session.SetInt32(ConstantValues.SessionCart,
                _shoppingCartManager.GetAll(shoppingCartViewModel.ApplicationUserId).Count());

            ClaimsIdentity? claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartDetailsViewModel = _shoppingCartManager.GetShoppingCartDetails(userId);

            return Json(new { shoppingCartViewModel, orderTotalString = shoppingCartDetailsViewModel.OrderTotalString });
        }

        [HttpPost]
        public IActionResult RemoveJson(int cartid)
        {
            _shoppingCartManager.Delete(cartid);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HttpContext.Session.SetInt32(ConstantValues.SessionCart, 
                _shoppingCartManager.GetAll(claim.Value).Count());

            var shoppingCartDetailsViewModel = _shoppingCartManager.GetShoppingCartDetails(claim.Value);

            return Json(shoppingCartDetailsViewModel);

        }
        #endregion
    }
}

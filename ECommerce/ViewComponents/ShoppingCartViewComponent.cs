using ECommerce.BusinessLogic.IManagers;
using ECommerce.Models.ViewModels;
using ECommerce.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartManager _shoppingCartManager;

        public ShoppingCartViewComponent(IShoppingCartManager shoppingCartManager)
        {
            _shoppingCartManager = shoppingCartManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var shoppingCarts =await _shoppingCartManager.GetAll(claim.Value);
                if (HttpContext.Session.GetInt32(ConstantValues.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(ConstantValues.SessionCart, shoppingCarts.Count());
                }

                return View(HttpContext.Session.GetInt32(ConstantValues.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}

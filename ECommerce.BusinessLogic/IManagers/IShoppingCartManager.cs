using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.IManagers
{
    public interface IShoppingCartManager
    {
        Task<List<ShoppingCartViewModel>> GetAll();
        Task<ShoppingCartViewModel> Get(int id);
        Task<int> Add(ShoppingCartViewModel shoppingCartViewModel);
        Task<int> Update(ShoppingCartViewModel shoppingCartViewModel);
        Task Delete(int id);
        Task<ShoppingCartViewModel> Get(int productId, string applicationUserId);
        Task<List<ShoppingCartViewModel>> GetAll(string applicationUserId);
        Task<ShoppingCartDetailsViewModel> GetShoppingCartDetails(string applicationUserId);
    }
}

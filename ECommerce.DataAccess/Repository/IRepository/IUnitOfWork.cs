using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository _categoryRepository { get; }
        IProductCategoryRepository _productCategoryRepository { get; }
        IProductRepository _productRepository { get; }
        IApplicationUserRepository _applicationUserRepository { get; }
        IProductImageRepository _productImageRepository { get; }
        IShoppingCartRepository _shoppingCartRepository { get; }
        Task Save();
    }
}

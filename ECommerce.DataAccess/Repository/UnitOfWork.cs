using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        public ICategoryRepository _categoryRepository { get; private set; }
        public IProductCategoryRepository _productCategoryRepository { get; private set; }
        public IProductRepository _productRepository { get; private set; }
        public IApplicationUserRepository _applicationUserRepository { get; private set; }
        public IProductImageRepository _productImageRepository { get; private set; }
        public IShoppingCartRepository _shoppingCartRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _categoryRepository = new CategoryRepository(_dbContext);
            _productCategoryRepository = new ProductCategoryRepository(_dbContext);
            _productRepository = new ProductRepository(_dbContext);
            _applicationUserRepository = new ApplicationUserRepository(_dbContext);
            _productImageRepository = new ProductImageRepository(_dbContext);
            _shoppingCartRepository = new ShoppingCartRepository(_dbContext);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();

            // make whole process async from controller to service layer
            //_dbContext.SaveChangesAsync();
        }
    }
}

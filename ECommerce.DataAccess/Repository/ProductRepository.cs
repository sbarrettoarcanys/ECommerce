using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductModel>> GetAllProducts(Expression<Func<ProductModel, bool>>? filter)
        {
            if (filter != null)
            {
                List<ProductModel> productModels = await _dbContext.Products
                                              .Where(filter)
                                              .Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                                              .Include(x => x.ProductImages)
                                              .AsNoTracking()
                                              .ToListAsync();
                return productModels;
            }
            else
            {
                List<ProductModel> productModels = await _dbContext.Products
                                              .Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                                              .Include(x => x.ProductImages)
                                              .AsNoTracking()
                                              .ToListAsync();
                return productModels;
            }

        }

        public void Update(ProductModel productModel)
        {
            productModel.UpdateDate = DateTime.Now;
            _dbContext.Entry(productModel).State = EntityState.Modified;
        }

        public void Delete(ProductModel productModel)
        {
            productModel.IsActive = false;
            Update(productModel);
        }

        public void DeleteProductCategories(ProductModel productModel)
        {
            _dbContext.ProductCategories.RemoveRange(productModel.ProductCategories);
        }
    }
}

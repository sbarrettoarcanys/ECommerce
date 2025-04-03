using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategoryModel>, IProductCategoryRepository
    {
        private ApplicationDbContext _dbContext;
        public ProductCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(ProductCategoryModel productCategoryModel)
        {
            productCategoryModel.UpdateDate = DateTime.Now;
            _dbContext.ProductCategories.Update(productCategoryModel);
        }

        public void Delete(ProductCategoryModel productCategoryModel)
        {
            productCategoryModel.IsActive = false;
            Update(productCategoryModel);
        }

        public void AddBulk(List<ProductCategoryModel> productCategoryModels)
        {
            _dbContext.ProductCategories.AddRange(productCategoryModels);
        }

        public void DeleteBulk(List<ProductCategoryModel> productCategoryModels)
        {
            foreach (ProductCategoryModel productCategoryModel in productCategoryModels)
            {
                productCategoryModel.IsActive = false;
            }
        }

        public void UpdateBulk(List<ProductCategoryModel> productCategoryModels)
        {
            foreach (ProductCategoryModel productCategoryModel in productCategoryModels)
            {
                productCategoryModel.UpdateDate = DateTime.Now;
            }
            _dbContext.ProductCategories.UpdateRange(productCategoryModels);
        }
    }
}

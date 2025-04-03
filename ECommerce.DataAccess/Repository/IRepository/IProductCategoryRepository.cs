using ECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategoryModel>
    {
        void Delete(ProductCategoryModel productCategoryModel);
        void Update(ProductCategoryModel productCategoryModel);
        void AddBulk(List<ProductCategoryModel> productCategoryModels);
        void DeleteBulk(List<ProductCategoryModel> productCategoryModels);
        void UpdateBulk(List<ProductCategoryModel> productCategoryModels);
    }
}

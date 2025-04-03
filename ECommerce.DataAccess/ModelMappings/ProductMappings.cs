using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using System.Linq.Expressions;

namespace ECommerce.DataAccess.ModelMappings
{

    public static class ProductCategoryMappings
    {


        public static readonly Expression<Func<ProductCategoryViewModel, ProductCategoryModel>> ProductCategoryViewModelToProductCategoryModel = x =>
        new ProductCategoryModel
        {

            Id = x.Id,
            CategoryId = x.CategoryId,
            IsActive = x.IsActive,
            CreateDate = x.CreateDate,
            UpdateDate = x.UpdateDate,
            ProductId = x.ProductId,
        };
    }
}

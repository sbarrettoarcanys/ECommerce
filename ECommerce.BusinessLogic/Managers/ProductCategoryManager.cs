using ECommerce.BusinessLogic.IManagers;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;

namespace ECommerce.BusinessLogic.Managers
{
    public class ProductCategoryManager : IProductCategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryManager _categoryManager;

        public ProductCategoryManager(IUnitOfWork unitOfWork, ICategoryManager categoryManager)
        {
            _unitOfWork = unitOfWork;
            _categoryManager = categoryManager;
        }

        public async Task<List<ProductCategoryViewModel>> CreateProductCategoryViewModels(int productId, IEnumerable<int> categoryIds)
        {
            List<ProductCategoryViewModel> productCategories = new List<ProductCategoryViewModel>();
            ProductViewModel productViewModel = await GetProduct(productId);

            foreach (var categoryId in categoryIds)
            {
                CategoryViewModel categoryViewModel = await _categoryManager.Get(categoryId);
                ProductCategoryViewModel productCategoryViewModel = new()
                {
                    CategoryId = categoryId,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                    ProductId = productId,

                };

                productCategories.Add(productCategoryViewModel);
            }

            return productCategories;
        }

        private async Task<ProductViewModel> GetProduct(int productId)
        {
            var products = await _unitOfWork._productRepository.GetAllProducts(x => x.Id == productId);
            ProductModel productModel = products.First();

            ProductViewModel productViewModel = productModel.Adapt<ProductViewModel>();
            return productViewModel;
        }
    }
}

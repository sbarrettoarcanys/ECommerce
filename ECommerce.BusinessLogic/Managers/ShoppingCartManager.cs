using ECommerce.BusinessLogic.IManagers;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.BusinessLogic.Managers
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryManager _categoryManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductImageManager _productImageManager;

        public ShoppingCartManager(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            IProductCategoryManager productCategoryManager,
            IProductImageManager productImageManager,
            ICategoryManager categoryManager)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _categoryManager = categoryManager;
            _productCategoryManager = productCategoryManager;
            _productImageManager = productImageManager;
        }

        public async Task<int> Add(ShoppingCartViewModel shoppingCartViewModel)
        {
            var existingCart = await Get(shoppingCartViewModel.ProductId, shoppingCartViewModel.ApplicationUserId);
            ShoppingCartModel shoppingCartModel = new ShoppingCartModel();
            if (existingCart != null)
            {
                //shopping cart exists
                existingCart.Count += shoppingCartViewModel.Count;
                shoppingCartModel = existingCart.Adapt<ShoppingCartModel>();

                shoppingCartModel.IsActive = true;
                _unitOfWork._shoppingCartRepository.Update(shoppingCartModel);
                await _unitOfWork.Save();
            }
            else
            {
                //add cart record
                shoppingCartModel = shoppingCartViewModel.Adapt<ShoppingCartModel>();
                shoppingCartModel.IsActive = true;
                shoppingCartModel.CreateDate = DateTime.Now;

                await _unitOfWork._shoppingCartRepository.Add(shoppingCartModel);
                await _unitOfWork.Save();
            }

            return shoppingCartModel.Id;
        }

        public async Task<ShoppingCartViewModel> Get(int id)
        {
            ShoppingCartModel shoppingCartModel = await _unitOfWork._shoppingCartRepository.Get(x => x.Id == id, "Product,ApplicationUser");
            ShoppingCartViewModel shoppingCartViewModel = shoppingCartModel.Adapt<ShoppingCartViewModel>();
            return shoppingCartViewModel;
        }
        public async Task<ShoppingCartViewModel> Get(int productId, string applicationUserId)
        {
            ShoppingCartModel shoppingCartModel = await _unitOfWork._shoppingCartRepository
                .Get(x => x.ProductId == productId && x.ApplicationUserId == applicationUserId, "Product,ApplicationUser");
            if (shoppingCartModel != null)
            {
                ShoppingCartViewModel shoppingCartViewModel = shoppingCartModel.Adapt<ShoppingCartViewModel>();
                return shoppingCartViewModel;
            }
            return null;
        }

        public async Task<List<ShoppingCartViewModel>> GetAll()
        {
            var shoppingCartModels = await _unitOfWork._shoppingCartRepository.GetAll(null, "Product,ApplicationUser");
            List<ShoppingCartViewModel> shoppingCartViewModels = shoppingCartModels.Adapt<List<ShoppingCartViewModel>>();
            return shoppingCartViewModels;
        }
        public async Task<List<ShoppingCartViewModel>> GetAll(string applicationUserId)
        {
            IEnumerable<ShoppingCartModel> shoppingCartModels = await _unitOfWork._shoppingCartRepository
                .GetAll(x => x.ApplicationUserId == applicationUserId, "Product,ApplicationUser");

            List<ShoppingCartViewModel> shoppingCartViewModels = shoppingCartModels.Adapt<List<ShoppingCartViewModel>>();
            return shoppingCartViewModels;
        }

        public async Task<int> Update(ShoppingCartViewModel shoppingCartViewModel)
        {
            ShoppingCartModel productModel = shoppingCartViewModel.Adapt<ShoppingCartModel>();
            _unitOfWork._shoppingCartRepository.Update(productModel);
            await _unitOfWork.Save();

            return productModel.Id;
        }

        public async Task Delete(int id)
        {
            ShoppingCartModel shoppingCartModel = await _unitOfWork._shoppingCartRepository.Get(x => x.Id == id, "Product,ApplicationUser");
            _unitOfWork._shoppingCartRepository.Delete(shoppingCartModel);
            await _unitOfWork.Save();
        }

        #region ShoppingCart details
        public async Task<ShoppingCartDetailsViewModel> GetShoppingCartDetails(string applicationUserId)
        {
            List<ShoppingCartViewModel> shoppingCartViewModels = await GetAll(applicationUserId);
            ShoppingCartDetailsViewModel shoppingCartDetailsViewModel = new ShoppingCartDetailsViewModel()
            {
                ShoppingCartViewModels = shoppingCartViewModels,
            };
            foreach (ShoppingCartViewModel cart in shoppingCartDetailsViewModel.ShoppingCartViewModels)
            {
                cart.Price = cart.Product.DiscountedPrice ?? cart.Product.Price;
                shoppingCartDetailsViewModel.OrderTotal += (cart.Price * cart.Count);

            }

            return shoppingCartDetailsViewModel;

        }
        #endregion
    }
}

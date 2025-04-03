using ECommerce.BusinessLogic.IManagers;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerce.BusinessLogic.Managers
{
    public class ProductImageManager : IProductImageManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductImageManager(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public void AddBulk(List<ProductImageViewModel> productViewModels)
        {
            List<ProductImageModel> productImageModels = productViewModels.Adapt<List<ProductImageModel>>();

            _unitOfWork._productImageRepository.AddBulk(productImageModels);
        }

        public async Task<List<ProductImageViewModel>> CreateProductImageViewModels(int productId, List<IFormFile> files)
        {

            List<ProductImageViewModel> productImages = new List<ProductImageViewModel>();
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            ProductViewModel productViewModel = await GetProduct(productId);

            foreach (IFormFile file in files)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = @"images\products\product-" + productId;
                string finalPath = Path.Combine(wwwRootPath, productPath);

                if (!Directory.Exists(finalPath))
                    Directory.CreateDirectory(finalPath);

                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                ProductImageViewModel productImage = new()
                {
                    ImageUrl = @"\" + productPath + @"\" + fileName,
                    ProductId = productId,
                    CreateDate = DateTime.Now,
                    IsActive = true,
                    ProductViewModel = productViewModel,

                };

                productImages.Add(productImage);

            }

            return productImages;
        }
        public async Task<int> Delete(int id)
        {
            ProductImageModel imageToBeDeleted = await _unitOfWork._productImageRepository.Get(u => u.Id == id);
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork._productImageRepository.Delete(imageToBeDeleted);
                await _unitOfWork.Save();
                return imageToBeDeleted.ProductId;
            }
            else
            {
                return 0;
            }
        }

        private async Task<ProductViewModel> GetProduct(int productId)
        {
            var products = await _unitOfWork._productRepository.GetAllProducts(x => x.Id == productId);
            ProductModel productModel = products.First();
            ProductViewModel productViewModel = productModel.Adapt<ProductViewModel>();
            return productViewModel;
        }

        public async Task<List<ProductImageViewModel>> GetAll(int productId)
        {
            IEnumerable<ProductImageModel> productImageModels = await _unitOfWork._productImageRepository.GetAll(x => x.ProductId == productId, "Product");
            List<ProductImageViewModel> productImageViewModels = productImageModels.Adapt<List<ProductImageViewModel>>();

            return productImageViewModels;
        }
    }
}

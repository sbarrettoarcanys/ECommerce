using ECommerce.BusinessLogic.IManagers;
using ECommerce.DataAccess.ModelMappings;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryManager _categoryManager;
        private readonly IProductCategoryManager _productCategoryManager;
        private readonly IProductImageManager _productImageManager;

        public ProductManager(IUnitOfWork unitOfWork,
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

        public async Task<List<ProductViewModel>> GetAll()
        {
            var allProducts = await _unitOfWork._productRepository.GetAllProducts(null);
            var products = allProducts.Adapt<List<ProductViewModel>>();
            return products;
        }

        public async Task<List<ProductViewModel>> GetAllActive()
        {
            var allActiveProducts = await _unitOfWork._productRepository.GetAllProducts(x => x.IsActive);

            var products = allActiveProducts.Adapt<List<ProductViewModel>>();


            return products;
        }
        public async Task<List<ProductViewModel>> GetAllActive(string searchText)
        {
            List<ProductModel> allActiveSearchedProducts = await _unitOfWork._productRepository
                .GetAllProducts(x => x.IsActive &&
                (x.Name.Contains(searchText) || x.Description.Contains(searchText) || x.Code.Contains(searchText)
                    || x.ProductCategories.Any(y => y.Category.Name.Contains(searchText)))

                );

            List<ProductViewModel> products = allActiveSearchedProducts.Adapt<List<ProductViewModel>>();

            return products;
        }


        public async Task<ProductViewModel> Get(int id)
        {
            var allProducts = await _unitOfWork._productRepository.GetAllProducts(x => x.Id == id);
            ProductModel productModel = allProducts.First();
            ProductViewModel productViewModel = productModel.Adapt<ProductViewModel>();

            return productViewModel;
        }

        public async Task<int> Add(ProductViewModel productViewModel, List<IFormFile> files)
        {
            productViewModel.CreateDate = DateTime.Now;
            productViewModel.IsActive = true;
            if (productViewModel.ProductCategories == null)
            {
                productViewModel.ProductCategories = new List<ProductCategoryViewModel>();
            }
            if (productViewModel.ProductImages == null)
            {
                productViewModel.ProductImages = new List<ProductImageViewModel>();
            }

            ProductModel productModel = productViewModel.Adapt<ProductModel>();

            await _unitOfWork._productRepository.Add(productModel);
            await _unitOfWork.Save();
            productViewModel.Id = productModel.Id;


            if (files != null && files.Count > 0)
            {
                List<ProductImageViewModel> productImageViewModels = await _productImageManager
                    .CreateProductImageViewModels(productViewModel.Id, files);
                _productImageManager.AddBulk(productImageViewModels);
                await _unitOfWork.Save();

            }

            if (productViewModel.CategoryIds != null && productViewModel.CategoryIds.Count() > 0)
            {
                List<ProductCategoryViewModel> productCategoryViewModels = await _productCategoryManager
                    .CreateProductCategoryViewModels(productViewModel.Id, productViewModel.CategoryIds);

                List<ProductCategoryModel> productCategoryModels = productCategoryViewModels.Adapt<List<ProductCategoryModel>>();

                productModel.ProductCategories.AddRange(productCategoryModels);
                await _unitOfWork.Save();
            }

            return productViewModel.Id;
        }

        public async Task<int> Update(ProductViewModel productViewModel, List<IFormFile> files)
        {

            if (productViewModel.ProductCategories == null)
            {
                productViewModel.ProductCategories = new List<ProductCategoryViewModel>();
            }
            if (productViewModel.ProductImages == null)
            {
                productViewModel.ProductImages = new List<ProductImageViewModel>();
            }

            ProductModel productModel = productViewModel.Adapt<ProductModel>();
            _unitOfWork._productRepository.Update(productModel);
            await _unitOfWork.Save();

            if (files != null && files.Count > 0)
            {
                var productImageViewModels = await _productImageManager
                    .CreateProductImageViewModels(productViewModel.Id, files);
                _productImageManager.AddBulk(productImageViewModels);
                await _unitOfWork.Save();

            }

            if (productViewModel.CategoryIds != null && productViewModel.CategoryIds.Count() > 0)
            {
                List<ProductCategoryViewModel> productCategoryViewModels = await _productCategoryManager
                           .CreateProductCategoryViewModels(productViewModel.Id, productViewModel.CategoryIds);

                await _unitOfWork._productCategoryRepository.SaveList<ProductCategoryViewModel>(
                   getAllFilter: x => x.ProductId == productViewModel.Id,
                   viewModelList: productCategoryViewModels,
                   compareFilter: (model, viewModel) => model.Id == productViewModel.Id && model.CategoryId == viewModel.CategoryId,
                   viewModelToModelExpression: x => ProductCategoryMappings.ProductCategoryViewModelToProductCategoryModel.Compile().Invoke(x)

                );

                await _unitOfWork.Save();
            }

            return productModel.Id;
        }


        public async Task Delete(int id)
        {
            ProductModel productModel = await _unitOfWork._productRepository.Get(x => x.IsActive && x.Id == id);
            _unitOfWork._productRepository.Delete(productModel);
            await _unitOfWork.Save();

        }

    }
}

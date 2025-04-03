using ECommerce.BusinessLogic.IManagers;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Mapster;

namespace ECommerce.BusinessLogic.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryViewModel>> GetAllActive()
        {
            IEnumerable<CategoryModel> categoryModels = await _unitOfWork._categoryRepository.GetAll(x => x.IsActive);
            List<CategoryViewModel> categories = categoryModels.Adapt<List<CategoryViewModel>>();

            return categories;
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            IEnumerable<CategoryModel> categoryModels = await _unitOfWork._categoryRepository.GetAll(null);
            List<CategoryViewModel> categories = categoryModels.Adapt<List<CategoryViewModel>>();

            return categories;
        }

        public async Task<CategoryViewModel> Get(int id)
        {
            CategoryModel categoryModel = await _unitOfWork._categoryRepository.Get(x => x.Id == id);
            CategoryViewModel categoryViewModel = categoryModel.Adapt<CategoryViewModel>();

            return categoryViewModel;
        }

        public async Task Add(CategoryViewModel categoryViewModel)
        {
            categoryViewModel.CreateDate = DateTime.Now;
            categoryViewModel.IsActive = true;
            CategoryModel categoryModel = categoryViewModel.Adapt<CategoryModel>();

            await _unitOfWork._categoryRepository.Add(categoryModel);
            await _unitOfWork.Save();
        }

        public async Task Update(CategoryViewModel categoryViewModel)
        {
            CategoryModel categoryModel = categoryViewModel.Adapt<CategoryModel>();
            _unitOfWork._categoryRepository.Update(categoryModel);
            await _unitOfWork.Save();
        }


        public async Task Delete(int id)
        {
            CategoryModel categoryModel = await _unitOfWork._categoryRepository.Get(x => x.IsActive && x.Id == id);
            _unitOfWork._categoryRepository.Delete(categoryModel);
            await _unitOfWork.Save();
        }
    }
}

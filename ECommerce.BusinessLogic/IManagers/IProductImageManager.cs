using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.IManagers
{
    public interface IProductImageManager
    {
        Task<int> Delete(int id);

        Task<List<ProductImageViewModel>> CreateProductImageViewModels(int productId, List<IFormFile> files);
        void AddBulk(List<ProductImageViewModel> productViewModels);

        Task<List<ProductImageViewModel>> GetAll(int productId);
    }
}

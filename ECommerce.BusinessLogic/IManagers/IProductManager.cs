using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.IManagers
{
    public interface IProductManager
    {
        Task<List<ProductViewModel>> GetAll();
        Task<List<ProductViewModel>> GetAllActive();
        Task<ProductViewModel> Get(int id);
        Task<int> Add(ProductViewModel productViewModel, List<IFormFile> files);
        Task<int> Update(ProductViewModel productViewModel, List<IFormFile> files);
        Task Delete(int id);
        Task<List<ProductViewModel>> GetAllActive(string searchText);
    }
}

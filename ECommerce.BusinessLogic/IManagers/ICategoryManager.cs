using ECommerce.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.IManagers
{
    public interface ICategoryManager
    {
        Task<List<CategoryViewModel>> GetAllActive();
        Task<List<CategoryViewModel>> GetAll();
        Task<CategoryViewModel> Get(int id);
        Task Add(CategoryViewModel categoryViewModel);
        Task Update(CategoryViewModel categoryViewModel);
        Task Delete(int id);
    }
}

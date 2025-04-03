using ECommerce.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BusinessLogic.IManagers
{
    public interface IProductCategoryManager
    {
        Task<List<ProductCategoryViewModel>> CreateProductCategoryViewModels(int productId, IEnumerable<int> categoryIds);

    }
}

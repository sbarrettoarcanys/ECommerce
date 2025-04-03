using ECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImageModel>
    {
        void Update(ProductImageModel productImageModel);
        void Delete(ProductImageModel productImageModel);
        void AddBulk(List<ProductImageModel> productImageModels);
    }
}

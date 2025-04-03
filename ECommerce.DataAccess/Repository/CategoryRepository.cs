using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
    {
        private ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(CategoryModel categoryModel)
        {
            categoryModel.UpdateDate = DateTime.Now;
            _dbContext.Categories.Update(categoryModel);
        }

        public void Delete(CategoryModel categoryModel)
        {
            categoryModel.IsActive = false;
            Update(categoryModel);
        }
    }
}

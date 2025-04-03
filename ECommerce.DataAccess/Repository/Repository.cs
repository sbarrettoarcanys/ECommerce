using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using ECommerce.Models.Models;
using ECommerce.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _dbContext;
        DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();

            _dbContext.Products.AsNoTracking().Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                               .Include(x => x.ProductImages);

            _dbContext.ProductCategories.AsNoTracking()
                                        .Include(x => x.Product)
                                        .Include(x => x.Category);

            _dbContext.Shoppingcarts.AsNoTracking()
                                    .Include(x => x.ApplicationUser)
                                    .Include(x => x.Product);

        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        //add isactive in filter from business logic layer
        public async Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public async Task SaveList<TViewModel>(Expression<Func<T, bool>> getAllFilter,
            List<TViewModel> viewModelList,
            Func<T, TViewModel, bool> compareFilter,
            Func<TViewModel, T> viewModelToModelExpression,
            Action<T, TViewModel> UpdateExistingModelExpression = null,
            Action<T> deleteExpression = null)
             where TViewModel : class
        {
            List<T> existingModels = _dbSet.Where(getAllFilter).AsNoTracking().ToList();

            List<T> newModels = new List<T>();
            List<T> deletedModels = new List<T>();

            existingModels?.ForEach(existingModel =>
            {
                var viewmodel = viewModelList?.FirstOrDefault(viewModel => compareFilter(existingModel, viewModel));
                if (viewmodel == null)
                {
                    deletedModels.Add(existingModel);
                }
            });

            viewModelList?.ForEach(viewModel =>
            {
                var existingModel = existingModels?.FirstOrDefault(model => compareFilter(model, viewModel));
                if (existingModel == null)
                {
                    newModels.Add(viewModelToModelExpression(viewModel));
                }
                else
                {
                    UpdateExistingModelExpression?.Invoke(existingModel, viewModel);
                }
            });


            if (newModels?.Count > 0)
            {
               await _dbSet.AddRangeAsync(newModels);
            }

            if (deletedModels?.Count > 0)
            {
                if (deleteExpression == null)
                {
                    _dbSet.RemoveRange(deletedModels);
                }
                else
                {
                    foreach (var deleteModel in deletedModels)
                    {
                        deleteExpression(deleteModel);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();


        }

    }
}

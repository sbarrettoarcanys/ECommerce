using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        public Task SaveList<TViewModel>(Expression<Func<T, bool>> getAllFilter,
            List<TViewModel> viewModelList,
            Func<T, TViewModel, bool> compareFilter,
            Func<TViewModel, T> viewModelToModelExpression,
            Action<T, TViewModel> UpdateExistingModelExpression = null,
            Action<T> deleteExpression = null) where TViewModel : class;
    }
}

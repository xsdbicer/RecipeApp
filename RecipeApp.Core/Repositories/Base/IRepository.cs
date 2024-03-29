﻿using System.Linq.Expressions;

namespace RecipeApp.Core.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T,bool>> expression);

        void SaveChange();
        Task SaveChangeAsync();


    }
}

using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CharityManagementBackend.Data.Base
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected charityContext RepositoryContext { get; set; }

        protected BaseRepository(charityContext _RepositoryContext)
        {
            RepositoryContext = _RepositoryContext;
        }

        public IEnumerable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            try
            {
                this.RepositoryContext.Set<T>().Add(entity);
            }
            catch (Exception)
            {

            }
        }

        public void CreateAsync(List<T> entities)
        {
            this.RepositoryContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Entry(entity).CurrentValues.SetValues(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            this.RepositoryContext.SaveChanges();
        }

        public void SaveAsync()
        {
            this.RepositoryContext.SaveChangesAsync().Wait();
        }
    }
}

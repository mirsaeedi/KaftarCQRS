using Microsoft.EntityFrameworkCore;
using System;

namespace Kaftar.Core.Data
{
    public class SetDataContext: ISetDataContext,IDisposable
    {
        private readonly DbContext _dbContext;

        public SetDataContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Set<TEntity>()
            where TEntity : class 
        {
            return _dbContext.Set<TEntity>();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class 
        {
            _dbContext.Update(entity);
        }

        public void UpdateRange<TEntity>(params TEntity[] entities) where TEntity : class 
        {
            _dbContext.UpdateRange(entities);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class 
        {
            _dbContext.Remove(entity);
        }

        public void RemoveRange<TEntity>(params TEntity[] entities) where TEntity : class 
        {
            _dbContext.RemoveRange(entities);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class 
        {
            _dbContext.Attach(entity);
        }

        public void AttachRange<TEntity>(params TEntity[] entities) where TEntity : class 
        {
            _dbContext.AttachRange(entities);
        }

        public void Entry<TEntity>(TEntity entity) where TEntity : class 
        {
            _dbContext.Entry(entity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.Data
{
    public class SetDataContext: ISetDataContext
    {
        private readonly DbContextBase _dbContext;

        public SetDataContext(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Set<TEntity>()
            where TEntity : class, IEntity
        {
            return _dbContext.Set<TEntity>();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _dbContext.Update(entity);
        }

        public void UpdateRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity
        {
            _dbContext.UpdateRange(entities);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _dbContext.Remove(entity);
        }

        public void RemoveRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity
        {
            _dbContext.RemoveRange(entities);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _dbContext.Attach(entity);
        }

        public void AttachRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity
        {
            _dbContext.AttachRange(entities);
        }

        public void Entry<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _dbContext.Entry(entity);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
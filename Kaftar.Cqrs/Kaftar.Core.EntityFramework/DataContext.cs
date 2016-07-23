using System.Data.Entity;
using System.Threading.Tasks;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public class DataContext : IDataContext
    {
        private readonly DbContextBase _dbContext;

        public DataContext(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }
        public DbSet<TEntity> Set<TEntity>() 
            where TEntity : class,IEntity
        {
            return _dbContext.Set<TEntity>();
        }
        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry<TEntity>(entity).State= EntityState.Modified;
        }
        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }

}
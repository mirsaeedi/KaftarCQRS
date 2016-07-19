using System.Data.Entity.Infrastructure;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public class ReadOnlyDataContext : IReadOnlyDataContext
    {
        private readonly DbContextBase _dbContext;

        public ReadOnlyDataContext(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public DbQuery<TEntity> Set<TEntity>() where TEntity : Entity
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }
    }
}
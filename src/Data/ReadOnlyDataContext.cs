using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kaftar.Core.Data
{
    public class ReadOnlyDataContext : IReadOnlyDataContext
    {
        private readonly DbContextBase _dbContext;

        public ReadOnlyDataContext(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }
    }
}
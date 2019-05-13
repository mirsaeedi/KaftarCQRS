using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kaftar.Core.Data
{
    public class ReadOnlyDataContext : IReadOnlyDataContext
    {
        private readonly DbContext _dbContext;

        public ReadOnlyDataContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }
    }
}
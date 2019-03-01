using Kaftar.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kaftar.Core.EntityFramework
{
    public class ReadOnlyDataContext : IReadOnlyDataContext
    {
        private readonly DbContextBase _dbContext;

        public ReadOnlyDataContext(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }
    }
}
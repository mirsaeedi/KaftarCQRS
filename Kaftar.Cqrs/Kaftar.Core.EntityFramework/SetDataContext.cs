using System.Data.Entity;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public class SetDataContext: ISetDataContext
    {
        private IDataContext DataContext { get; set; }

        internal SetDataContext(IDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity
        {
            return DataContext.Set<TEntity>();
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            DataContext.Set<TEntity>().Attach(entity);
        }

    }
}
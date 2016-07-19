using System.Data.Entity;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public interface ISetDataContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
        void Update<TEntity>(TEntity entity) where TEntity : Entity;
    }
}
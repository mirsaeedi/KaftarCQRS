using Kaftar.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.EntityFramework
{
    public interface ISetDataContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;
    }
}
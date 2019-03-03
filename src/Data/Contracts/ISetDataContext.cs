using Microsoft.EntityFrameworkCore;
using System;

namespace Kaftar.Core.Data
{
    public interface ISetDataContext:IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity;

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void AttachRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity;
        
        void Entry<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void RemoveRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity;

        void Remove<TEntity>(TEntity entity) where TEntity : class, IEntity;

        void UpdateRange<TEntity>(params TEntity[] entities) where TEntity : class, IEntity;
    }
}
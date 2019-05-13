using Microsoft.EntityFrameworkCore;
using System;

namespace Kaftar.Core.Data
{
    public interface ISetDataContext:IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class ;

        void Update<TEntity>(TEntity entity) where TEntity : class ;

        void Attach<TEntity>(TEntity entity) where TEntity : class ;

        void AttachRange<TEntity>(params TEntity[] entities) where TEntity : class ;
        
        void Entry<TEntity>(TEntity entity) where TEntity : class ;

        void RemoveRange<TEntity>(params TEntity[] entities) where TEntity : class ;

        void Remove<TEntity>(TEntity entity) where TEntity : class ;

        void UpdateRange<TEntity>(params TEntity[] entities) where TEntity : class ;
    }
}
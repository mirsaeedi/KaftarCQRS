using System;
using System.Linq;

namespace Kaftar.Core.Data
{
    public interface IReadOnlyDataContext:IDisposable
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity;
    }
}
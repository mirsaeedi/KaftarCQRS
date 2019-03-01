using Kaftar.Core.Models;
using System.Linq;

namespace Kaftar.Core.EntityFramework
{
    public interface IReadOnlyDataContext
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity;
    }
}
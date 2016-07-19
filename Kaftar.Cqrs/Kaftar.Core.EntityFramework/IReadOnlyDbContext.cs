using System.Data.Entity.Infrastructure;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public interface IReadOnlyDataContext
    {
        DbQuery<TEntity> Set<TEntity>() where TEntity : Entity;
    }
}
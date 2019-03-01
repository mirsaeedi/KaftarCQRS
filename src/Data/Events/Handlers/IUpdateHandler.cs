using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface IUpdateHandler<in T> where T:IEntity
    {
        void PreUpdate(T entity);

        void PostUpdate(T entity);
    }
}

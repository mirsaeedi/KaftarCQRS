using Kaftar.Core.Data;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface IUpdateHandler<in T> where T:IEntity
    {
        void PreUpdate(T entity);

        void PostUpdate(T entity);
    }
}

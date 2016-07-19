using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface IDeleteHandler<T> where T:Entity
    {
        void PreDelete(T entity);

        void PostDelete(T entity);
    }
}

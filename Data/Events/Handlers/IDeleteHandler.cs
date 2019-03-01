using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface IDeleteHandler<T> where T:IEntity
    {
        void PreDelete(T entity);

        void PostDelete(T entity);
    }
}

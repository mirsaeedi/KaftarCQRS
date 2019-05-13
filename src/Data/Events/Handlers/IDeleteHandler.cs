using Kaftar.Core.Data;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface IDeleteHandler<T> where T:class
    {
        void PreDelete(T entity);

        void PostDelete(T entity);
    }
}

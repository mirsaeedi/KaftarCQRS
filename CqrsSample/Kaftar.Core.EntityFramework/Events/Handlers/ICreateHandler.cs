using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface ICreateHandler<T> where T:Entity
    {
        void PreCreate(T entity);

        void PostCreate(T entity);
    }
}

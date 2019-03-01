using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework.Events.Handlers
{
    public interface ICreateHandler<T> where T:IEntity
    {
        void PreCreate(T entity);

        void PostCreate(T entity);
    }
}

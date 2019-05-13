
namespace Kaftar.Core.Data.Events   
{
    public interface ICreateHandler<T> where T: class
    {
        void PreCreate(T entity);

        void PostCreate(T entity);
    }
}

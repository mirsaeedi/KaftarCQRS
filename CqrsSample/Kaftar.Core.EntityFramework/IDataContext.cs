using System.Threading.Tasks;

namespace Kaftar.Core.EntityFramework
{
    public interface IDataContext:ISetDataContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
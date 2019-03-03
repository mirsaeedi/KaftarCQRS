using System;
using System.Threading.Tasks;

namespace Kaftar.Core.Data
{
    public interface IDataContext:IDisposable
    {
        ISetDataContext Data { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
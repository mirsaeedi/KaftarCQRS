using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.Data
{
    internal class DataContext : IDataContext
    {
        private DbContextBase _dbContext;

        public ISetDataContext Data { get; }

        internal DataContext(ISetDataContext setDataContext, DbContextBase dbContext)
        {
            Data = setDataContext;
            _dbContext = dbContext;
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Data.Dispose();
            _dbContext.Dispose();
        }
    }

}
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.Data
{
    internal class DataContext : IDataContext
    {
        private DbContext _dbContext;

        public ISetDataContext Data { get; }

        public DataContext(ISetDataContext setDataContext, DbContext dbContext)
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
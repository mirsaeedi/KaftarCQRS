using System.Linq;
using System.Threading.Tasks;
using Kaftar.Core.CQRS.QueryStack.QueryHandler;
using Kaftar.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.Cqrs.CRUD
{
    public class ReadQueryHandler<TEntity>:QueryHandler<ReadCqrsQuery<TEntity>,TEntity[]>
        where TEntity: class, IEntity
    {
        protected override async Task<TEntity[]> GetResult(ReadCqrsQuery<TEntity> query)
        {
            var set = DataContext.Set<TEntity>();
            var configurationExpression = query.QueryConfiguration(set);

            return await set.Concat(configurationExpression)
                .ToArrayAsync();
        }
    }
}
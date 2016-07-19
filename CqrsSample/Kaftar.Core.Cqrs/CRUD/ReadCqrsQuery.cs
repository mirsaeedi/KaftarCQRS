using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{
    public class ReadCqrsQuery<TEntity>:CqrsQuery
        where TEntity:Entity
    {
        public ReadCqrsQuery()
        {
            // set default query
            QueryConfiguration = (query) => query;
        }
        public TEntity Entity { get; set; }

        public Func<DbQuery<TEntity>, IQueryable<TEntity>> QueryConfiguration;
    }

}

using System;
using System.Threading.Tasks;
using Autofac;
using Kaftar.Core.Cqrs.CRUD;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.Cqrs.QueryStack.QueryResults;
using Kaftar.Core.CQRS.QueryStack.QueryHandler;
using Kaftar.Core.EntityFramework;

namespace Kaftar.Core.Cqrs.QueryStack
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _context;
        public QueryDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<CqrsQueryResult<TQueryValueResult>> Dispatch<TQuery, TQueryValueResult>(TQuery query, int userId, string ip)
            where TQuery : CqrsQuery
            where TQueryValueResult : class
        {

            query.IpAddress = ip;
            query.UserId = userId;

            var dataContext = _context.Resolve<IReadOnlyDataContext>();

            if (query.GetType().GetGenericTypeDefinition() == typeof(ReadCqrsQuery<>))
            {
                var handlerType = typeof(ReadQueryHandler<>);
                return await HandlerCrudQuery<TQuery, TQueryValueResult>(handlerType, query, dataContext);
            }
            else
            {
                var handler = _context.Resolve<QueryHandler<TQuery, TQueryValueResult>>();
                handler.DataContext = dataContext;
                return await handler.Execute(query);
            }


        }

        private async Task<CqrsQueryResult<TValue>> HandlerCrudQuery<TQuery, TValue>(Type handlerType, TQuery query, IReadOnlyDataContext dataContext)
        {
            Type[] typeArgs = { typeof(TQuery).GetGenericArguments()[0] };
            var genericHandlerType = handlerType.MakeGenericType(typeArgs);

            dynamic handler = Activator.CreateInstance(genericHandlerType);
            handler.DataContext = dataContext;
            return await handler.Execute(query);
        }
    }
}
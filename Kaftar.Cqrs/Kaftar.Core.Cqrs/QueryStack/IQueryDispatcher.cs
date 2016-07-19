using System.Threading.Tasks;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.Cqrs.QueryStack.QueryResults;

namespace Kaftar.Core.Cqrs.QueryStack
{
    public interface IQueryDispatcher
    {
        Task<CqrsQueryResult<TQueryValueResult>> Dispatch<TQuery, TQueryValueResult>(TQuery query, int userId, string ip)
            where TQuery : CqrsQuery
            where TQueryValueResult : class;
    }
}
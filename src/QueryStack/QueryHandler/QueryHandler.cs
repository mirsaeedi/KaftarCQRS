using System;
using System.Threading.Tasks;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.Cqrs.QueryStack.QueryResults;
using Kaftar.Core.EntityFramework;

namespace Kaftar.Core.CQRS.QueryStack.QueryHandler
{
    public abstract class QueryHandler<TQuery, TQueryValueResult> : IQueryHandler<TQuery, TQueryValueResult>
                    where TQuery : CqrsQuery
                    where TQueryValueResult : class
    {
        public IReadOnlyDataContext DataContext { get; internal set; }

        public async Task<CqrsQueryResult<TQueryValueResult>> Execute(TQuery query)
        {
            try
            {
                if (ActivityAuthorizationIsConfirmed(query))
                {
                    SaveQuery(query);

                    var queryResult = PreExecutionValidation(query);

                    if (queryResult.MetaData.WasSuccesfull)
                    {
                        var value = await GetResult(query);
                        queryResult.Value = value;
                        PostExecutionValidation(query, value, queryResult);
                    }

                    SaveQueryResult(queryResult);

                    if (queryResult.MetaData.WasSuccesfull)
                        OnSucess(query, queryResult);
                    else
                        OnFail(null, query, queryResult);

                    return queryResult;
                }
                else
                {
                    throw new Exception("Not Authorized");
                }
            }
            catch (Exception exception)
            {
                return HandleFailed(exception, query);
            }
            finally
            {

            }
        }

        private CqrsQueryResult<TQueryValueResult> HandleFailed(Exception exception, TQuery query)
        {
            return null;
        }

        protected virtual bool ActivityAuthorizationIsConfirmed(TQuery query)
        {
            return true;
        }

        protected virtual void PostExecutionValidation(TQuery query, TQueryValueResult value, CqrsQueryResult<TQueryValueResult> queryResult)
        {

        }

        protected virtual async Task<TQueryValueResult> GetResult(TQuery query) { return default(TQueryValueResult); }

        protected virtual CqrsQueryResult<TQueryValueResult> PreExecutionValidation(TQuery query)
        {
            return OkResult(query);
        }

        protected virtual void SaveQuery(TQuery query) { }

        protected virtual void SaveQueryResult(CqrsQueryResult<TQueryValueResult> queryResult) { }

        protected virtual void OnSucess(TQuery query, CqrsQueryResult<TQueryValueResult> queryResult) { }

        protected virtual void OnFail(Exception exception, TQuery query, CqrsQueryResult<TQueryValueResult> queryResult) { }

        protected virtual CqrsQueryResult<TQueryValueResult> CreateFailedResult(Exception exception, TQuery query)
        {
            var result = new CqrsQueryResult<TQueryValueResult>(-100, exception.ToString(), query, null);
            return result;
        }

        protected CqrsQueryResult<TQueryValueResult> OkResult(TQuery query)
        {
            return new CqrsQueryResult<TQueryValueResult>(0, query, null);
        }

        protected CqrsQueryResult<TQueryValueResult> FailedExceptionResult(TQuery query)
        {
            return new CqrsQueryResult<TQueryValueResult>(-1, "Unhandled Exception", query, null);
        }

    }
}
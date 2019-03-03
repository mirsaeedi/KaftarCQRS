using System;
using System.Threading.Tasks;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.Cqrs.QueryStack.QueryResults;
using Kaftar.Core.Data;

namespace Kaftar.Core.CQRS.QueryStack.QueryHandler
{
    public abstract class QueryHandler<TQuery, TQueryValueResult> : IQueryHandler<TQuery, TQueryValueResult>
                    where TQuery : CqrsQuery
                    where TQueryValueResult : class
    {
        public IReadOnlyDataContext DataContext { get; internal set; }

        public async Task<CqrsQueryResult<TQueryValueResult>> Execute(TQuery query)
        {
            CqrsQueryResult<TQueryValueResult> queryResult = default;

            try
            {
                SaveQuery(query);

                if (await ActivityAuthorizationIsConfirmed(query))
                {
                    queryResult = await PreExecutionValidation(query);

                    if (queryResult.MetaData.WasSuccesfull)
                    {
                        queryResult.Value = await GetResult(query);
                    }

                    queryResult.MetaData.ResultDateTime = DateTime.Now;

                    if (queryResult.MetaData.WasSuccesfull)
                        await OnSucess(query, queryResult);
                    else
                        await OnFail(null, query, queryResult);
                }
                else
                {
                    throw new Exception("Not Authorized");
                }

            }
            catch (Exception exception)
            {
                queryResult = await HandleFailed(exception, query);
            }
            finally
            {
                SaveQueryResult(queryResult);
            }

            return queryResult;

        }

        #region Template


        protected virtual Task<bool> ActivityAuthorizationIsConfirmed(TQuery query)
        {
            return Task.FromResult(true);
        }

        protected virtual async Task<TQueryValueResult> GetResult(TQuery query) { return default(TQueryValueResult); }

        protected virtual Task<CqrsQueryResult<TQueryValueResult>> PreExecutionValidation(TQuery query)
        {
            return Task.FromResult(Ok(query));
        }

        protected virtual Task OnSucess(TQuery query, CqrsQueryResult<TQueryValueResult> queryResult) { return Task.CompletedTask; }

        protected virtual Task OnFail(Exception exception, TQuery query, CqrsQueryResult<TQueryValueResult> queryResult) { return Task.CompletedTask; }


        #endregion

        protected CqrsQueryResult<TQueryValueResult> Ok(TQuery query)
        {
            return new CqrsQueryResult<TQueryValueResult>(0, query, default(TQueryValueResult));
        }

        private async Task<CqrsQueryResult<TQueryValueResult>> HandleFailed(Exception exception, TQuery query)
        {
            var queryResult = new CqrsQueryResult<TQueryValueResult>(-1,query,default);
            await OnFail(exception, query, queryResult);

            return queryResult;
        }


        #region Save Query and QeuryResult

        protected virtual void SaveQuery(TQuery query) { }

        protected virtual void SaveQueryResult(CqrsQueryResult<TQueryValueResult> queryResult) { }


        #endregion
    }

}
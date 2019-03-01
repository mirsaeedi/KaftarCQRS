using System;
using CqrsSample.Core.CQRS;
using Kaftar.Core.Cqrs.QueryStack.Queries;

namespace Kaftar.Core.Cqrs.QueryStack.QueryResults
{
    public class CqrsQueryResult<T>
    {
        public T Value { get; set; }

        public CqrsMessageResultMetaData MetaData { get; private set; }

        public CqrsQueryResult(int resultCode, string description, CqrsQuery query, T value)
        {
            MetaData = new CqrsMessageResultMetaData(resultCode, description, DateTime.Now, query.Guid);

            Value = value;
        }

        public CqrsQueryResult(int resultCode, CqrsQuery query, T value)
            :this(resultCode,null,query,value)
        {
        }
    }
}
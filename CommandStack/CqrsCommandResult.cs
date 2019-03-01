using System;
using CqrsSample.Core.CQRS;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public class CqrsCommandResult
    {
        public CqrsMessageResultMetaData MetaData { get; private set; }

        public CqrsCommandResult(int resultCode, string description, CqrsCommand command)
        {
            MetaData = new CqrsMessageResultMetaData(resultCode, description, DateTime.Now, command.Guid);
        }

        public CqrsCommandResult(int resultCode, CqrsCommand command)
            :this(resultCode,null,command)
        {
        }
    }
}
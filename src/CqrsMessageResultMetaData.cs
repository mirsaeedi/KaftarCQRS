using System;

namespace CqrsSample.Core.CQRS
{
    public class CqrsMessageResultMetaData
    {
        internal CqrsMessageResultMetaData(int resultCode,string description,DateTime dateTime,Guid commandGuid)
        {
            ResultCode = resultCode;
            ResultDescription = description;
            ResultDateTime = dateTime;
            CommandGuid = commandGuid;
        }

        public int ResultCode { get; set; }

        public string ResultDescription { get; set; }

        public DateTime ResultDateTime { get; internal set; }

        public Guid CommandGuid { get; private set; }

        public bool WasSuccesfull => ResultCode == 0;
    }
}
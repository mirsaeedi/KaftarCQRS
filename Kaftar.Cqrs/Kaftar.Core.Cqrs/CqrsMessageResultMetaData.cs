using System;

namespace CqrsSample.Core.CQRS
{
    public class CqrsMessageResultMetaData
    {
        internal CqrsMessageResultMetaData(int resultCode,string description,DateTime dateTime,Guid messageGuid)
        {
            ResultCode = resultCode;
            ResultDescription = description;
            ResultDateTime = dateTime;
            MessageGuid = messageGuid;
        }

        public int ResultCode { get; private set; }

        public string ResultDescription { get; private set; }

        public DateTime ResultDateTime { get; private set; }

        public Guid MessageGuid { get; private set; }

        public bool WasSuccesfull => ResultCode == 0;

        public bool ServerExceptionOccured => ResultCode < 0;

        public bool ValidationFailed => ResultCode > 0;
    }
}
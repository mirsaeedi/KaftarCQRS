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

        public int ResultCode { get; set; }

        public string ResultDescription { get; set; }

        public DateTime ResultDateTime { get; internal set; }

        public Guid MessageGuid { get; private set; }

        public bool WasSuccesfull => ResultCode == 0;

        public bool ServerExceptionOccured => ResultCode < 0;

        public bool ValidationFailed => ResultCode > 0;

        public bool PersistData => ResultCode >= 0 && ResultCode<500 ;
    }
}
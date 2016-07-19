using System;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public abstract class CqrsCommand : ICqrsMessage
    {
        public DateTime IssueDateTime { get; set; }

        public Guid Guid { get; set; }

        public string IpAddress { get; set; }

        public long UserId { get; set; }
    }
}
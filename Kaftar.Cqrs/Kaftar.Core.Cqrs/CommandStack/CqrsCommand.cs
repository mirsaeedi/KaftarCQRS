using System;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public abstract class CqrsCommand : ICqrsMessage
    {
        public CqrsCommand()
        {
            Guid = Guid.NewGuid();
            IssueDateTime = DateTime.Now;
        }
        public DateTime IssueDateTime { get; set; }

        public Guid Guid { get; set; }

        public string IpAddress { get; set; }

        public long UserId { get; set; }
    }
}
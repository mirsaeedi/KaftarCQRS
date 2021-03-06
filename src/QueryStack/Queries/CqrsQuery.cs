﻿using System;

namespace Kaftar.Core.Cqrs.QueryStack.Queries
{
    public abstract class CqrsQuery: ICqrsMessage
    {
        public CqrsQuery()
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaftar.Core.Cqrs
{
    interface ICqrsMessage
    {
        DateTime IssueDateTime { get; set; }

        Guid Guid { get; set; }

        string IpAddress { get; set; }

        long UserId { get; set; }
    }
}

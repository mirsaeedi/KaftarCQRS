using System;

namespace Kaftar.Core.Data
{
    public interface IAuditableEntity:IEntity
    {
        long? LastModifierId { get; set; }

        long? CreatorId { get; set; }

        DateTime LastModifiedDateTime { get;  set; }

        DateTime CreateDateTime { get;  set; }
    }
}
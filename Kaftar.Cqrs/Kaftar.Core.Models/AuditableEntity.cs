using System;

namespace Kaftar.Core.Models
{
    public class AuditableEntity:Entity
    {
        protected internal long LastModifierId { get; set; }

        protected internal long CreatorId { get; set; }

        protected internal DateTime LastModifiedDateTime { get; set; }

        protected internal DateTime CreateDateTime { get; set; }
    }
}
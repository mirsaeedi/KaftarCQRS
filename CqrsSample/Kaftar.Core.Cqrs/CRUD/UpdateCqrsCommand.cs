using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{
    public class UpdateCqrsCommand<TEntity>:CqrsCommand
        where TEntity : Entity
    {
        public TEntity Entity { get; set; }
    }
}
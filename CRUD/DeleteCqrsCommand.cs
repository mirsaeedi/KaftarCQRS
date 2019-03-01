using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{
    public class DeleteCqrsCommand<TEntity>:CqrsCommand
        where TEntity : IEntity
    {
        public TEntity Entity { get; set; }
    }
}
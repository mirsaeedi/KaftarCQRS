using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Data;

namespace Kaftar.Core.Cqrs.CRUD
{
    public class DeleteCqrsCommand<TEntity>:CqrsCommand
    {
        public TEntity Entity { get; set; }
    }
}
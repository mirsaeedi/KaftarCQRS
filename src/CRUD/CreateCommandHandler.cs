using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Data;

namespace Kaftar.Core.Cqrs.CRUD
{
    internal class CreateCommandHandler<TEntity> : CommandHandler<CreateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : class, IEntity
    {

        protected override async Task Handle(CreateCqrsCommand<TEntity> cqrsCommand)
        {
            SetDataContext.Set<TEntity>().Add(cqrsCommand.Entity);
        }
    }
}
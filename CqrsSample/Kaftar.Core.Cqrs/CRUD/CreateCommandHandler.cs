using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CommandStack.CommandHandlers;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{
    internal class CreateCommandHandler<TEntity> : CommandHandler<CreateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : Entity
    {
        protected override CqrsCommandResult PreExecutionValidation(CreateCqrsCommand<TEntity> cqrsCommand)
        {
            return OkResult(cqrsCommand);
        }

        protected override async Task Handle(CreateCqrsCommand<TEntity> cqrsCommand)
        {
            SetDataContext.Set<TEntity>().Add(cqrsCommand.Entity);
        }
    }
}
using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CommandStack.CommandHandlers;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{

    internal class UpdateCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : Entity
    {
        protected override CqrsCommandResult PreExecutionValidation(UpdateCqrsCommand<TEntity> command)
        {
            return OkResult(command);
        }

        protected override async Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            SetDataContext.Update(command.Entity);
        }
    }
}
using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{

    internal class UpdateCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : class, IEntity
    {
        protected override async Task<CqrsCommandResult> PreExecutionValidation(UpdateCqrsCommand<TEntity> command)
        {
            return OkResult();
        }

        protected override async Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            SetDataContext.Update(command.Entity);
        }
    }
}
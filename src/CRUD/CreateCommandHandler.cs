using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CRUD
{
    internal class CreateCommandHandler<TEntity> : CommandHandler<CreateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : class, IEntity
    {
        protected override async Task<CqrsCommandResult> PreExecutionValidation(CreateCqrsCommand<TEntity> cqrsCommand)
        {
            return OkResult();
        }

        protected override async Task Handle(CreateCqrsCommand<TEntity> cqrsCommand)
        {
            SetDataContext.Set<TEntity>().Add(cqrsCommand.Entity);
        }
    }
}
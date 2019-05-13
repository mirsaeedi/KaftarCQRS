using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CRUD;
using Kaftar.Core.EntityFramework;
using Kaftar.Core.Data;

namespace CqrsSample.Core.CQRS.CommandStack.CommandHandlers.CRUDCommandHandlers
{
    internal class DeleteCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
    {
        protected override Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            return Task.CompletedTask;
        }
    }
}
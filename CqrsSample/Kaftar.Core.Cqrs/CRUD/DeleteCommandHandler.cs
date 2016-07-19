using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CommandStack.CommandHandlers;
using Kaftar.Core.Cqrs.CRUD;
using Kaftar.Core.EntityFramework;
using Kaftar.Core.Models;

namespace CqrsSample.Core.CQRS.CommandStack.CommandHandlers.CRUDCommandHandlers
{
    internal class DeleteCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : Entity
    {

        public DeleteCommandHandler() 
        {

        }

        protected override CqrsCommandResult PreExecutionValidation(UpdateCqrsCommand<TEntity> command)
        {
            return OkResult(command);
        }

        protected override Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            return null;
        }
    }
}
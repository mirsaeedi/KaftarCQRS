using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CommandStack.CommandHandlers;
using Kaftar.Core.Cqrs.CRUD;
using Kaftar.Core.EntityFramework;
using Kaftar.Core.Models;

namespace CqrsSample.Core.CQRS.CommandStack.CommandHandlers.CRUDCommandHandlers
{
    internal class DeleteCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : IEntity
    {

        public DeleteCommandHandler() 
        {

        }

        protected override async Task<CqrsCommandResult> PreExecutionValidation(UpdateCqrsCommand<TEntity> command)
        {
            return OkResult();
        }

        protected override Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            return null;
        }
    }
}
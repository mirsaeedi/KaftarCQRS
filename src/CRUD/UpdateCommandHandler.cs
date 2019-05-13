using System.Threading.Tasks;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Data;

namespace Kaftar.Core.Cqrs.CRUD
{

    internal class UpdateCommandHandler<TEntity> : CommandHandler<UpdateCqrsCommand<TEntity>, CqrsCommandResult>
        where TEntity : class
    {

        protected override async Task Handle(UpdateCqrsCommand<TEntity> command)
        {
            SetDataContext.Update(command.Entity);
        }
    }
}
using System.Threading.Tasks;

namespace Kaftar.Core.Cqrs.CommandStack.CommandHandlers
{
    public interface ICommandHandler<in TCommand, TCommandResult>
          where TCommand : CqrsCommand
          where TCommandResult : CqrsCommandResult
    {
        Task<TCommandResult> Execute(TCommand command);
    }    
    
}
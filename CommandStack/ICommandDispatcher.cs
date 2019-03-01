using System.Threading.Tasks;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public interface ICommandDispatcher
    {
        Task<CqrsCommandResult> Dispatch<TCommand>(TCommand command, int userId, string ip) where TCommand : CqrsCommand;
    }
}
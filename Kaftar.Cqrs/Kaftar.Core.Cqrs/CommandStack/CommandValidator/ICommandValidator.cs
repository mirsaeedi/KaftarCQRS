namespace Kaftar.Core.Cqrs.CommandStack.CommandValidator
{
    internal interface ICommandValidator<in TCommand>
        where TCommand : CqrsCommand
    {
        CqrsCommandResult IsValid(TCommand command, CqrsCommandResult commandResult);
    }
}

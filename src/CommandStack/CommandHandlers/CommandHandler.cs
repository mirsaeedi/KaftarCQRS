using System;
using System.Threading.Tasks;
using Kaftar.Core.EntityFramework;
using Kaftar.Core.Models;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public abstract class CommandHandler<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
        where TCommand : CqrsCommand
        where TCommandResult : CqrsCommandResult
    {
        internal IDataContext InnerDataContext { get;  set; }
        protected ISetDataContext SetDataContext { get; private set; }
        protected bool ParentOfChain { get; set; }
        internal IEntity CommandEntity { get; set; }
        internal TCommand Command { get; set; }

        public CommandHandler()
        {
            ParentOfChain = true;
        }

        public async Task<TCommandResult> Execute(TCommand command)
        {
            Command = command;

            try
            {
                SetDataContext = new SetDataContext(InnerDataContext);

                if (ActivityAuthorizationIsConfirmed(command))
                {
                    SaveCommand(command);

                    GetLock();

                    var commandResult = await PreExecutionValidation(command);

                    if (commandResult.MetaData.WasSuccesfull)
                    {
                        await Handle(command);
                        await PostExecutionValidate(command, commandResult);
                    }

                    commandResult.MetaData.ResultDateTime = DateTime.Now;

                    SaveCommandResult(commandResult);

                    if(commandResult.MetaData.PersistData && ParentOfChain)
                        InnerDataContext.SaveChanges();

                    if (commandResult.MetaData.WasSuccesfull)
                        await OnSucess(command, commandResult);
                    else
                        await OnFail(null, command, commandResult);

                    return commandResult;
                }
                else
                {
                    throw new Exception("Not Authorized");
                }
            }
            catch (Exception exception)
            {
                return HandleFailed(exception, command);
            }
            finally
            {
                ReleaseLock();
            }
        }

        protected virtual void GetLock()
        {

        }

        protected virtual void ReleaseLock()
        {

        }

        private TCommandResult HandleFailed(Exception exception, TCommand command)
        {
            return null;
        }

        protected virtual bool ActivityAuthorizationIsConfirmed(TCommand command)
        {
            return true;
        }

        protected abstract Task Handle(TCommand command);

        protected virtual void SaveCommand(TCommand command) { }

        protected virtual void SaveCommandResult(TCommandResult commandResult) { }

        protected virtual async Task OnSucess(TCommand command, TCommandResult commandResult) { }

        protected virtual async Task OnFail(Exception exception, TCommand command, TCommandResult commandResult) {  }

        protected virtual TCommandResult CreateFailedResult(Exception exception, TCommand command)
        {
            return new CqrsCommandResult(-100, exception.ToString(),command) as TCommandResult;
        }

        protected virtual async Task PostExecutionValidate(TCommand command, TCommandResult commandResult) {  }

        protected virtual async Task<TCommandResult> PreExecutionValidation(TCommand command)
        {
            return OkResult();
        }

        protected TCommandResult OkResult()
        {
            return Activator.CreateInstance(typeof(TCommandResult), 0, null, Command)
                as TCommandResult;
        }

        protected TCommandResult FailedExceptionResult()
        {
            return Activator.CreateInstance(typeof(TCommandResult), 0, null, Command)
                as TCommandResult;
        }

    }
}
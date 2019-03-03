using System;
using System.Linq;
using System.Threading.Tasks;
using Kaftar.Core.Cqrs.QueryStack.Queries;
using Kaftar.Core.EntityFramework;
using Kaftar.Core.Data;
using CqrsSample.Core.CQRS;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public abstract class CommandHandler<TCommand, TCommandResult> : ICommandHandler<TCommand, TCommandResult>
        where TCommand : CqrsCommand
        where TCommandResult : CqrsCommandResult,new()
    {
        internal IDataContext DataContext { get;  set; }
        protected ISetDataContext SetDataContext { get; private set; }
        protected IEntity CommandEntity { get; set; }

        public async Task<TCommandResult> Execute(TCommand command)
        {
            SaveCommand(command);
            TCommandResult commandResult = default;

            try
            {
                if (await ActivityAuthorizationIsConfirmed(command))
                {
                    GetLock();

                    commandResult = await PreExecutionValidation(command);

                    if (commandResult.MetaData.WasSuccesfull)
                    {
                        await Handle(command);
                        await PostExecutionValidate(command, commandResult);
                    }

                    commandResult.MetaData.ResultDateTime = DateTime.Now;

                    if (commandResult.MetaData.WasSuccesfull)
                       await DataContext.SaveChangesAsync();

                    if (commandResult.MetaData.WasSuccesfull)
                        await OnSucess(command, commandResult);
                    else
                        await OnFail(null, command, commandResult);
                }
                else
                {
                    throw new Exception("Not Authorized");
                }
            }
            catch (Exception exception)
            {
                commandResult = await HandleFailed(exception, command);
            }
            finally
            {
                SaveCommandResult(commandResult);
                ReleaseLock();
            }

            return commandResult;
        }

        #region Concurrency Mechanism

        protected virtual void GetLock()
        {

        }

        protected virtual void ReleaseLock()
        {

        }

        #endregion

        private async Task<TCommandResult> HandleFailed(Exception exception, TCommand command)
        {
            var commandResult = new TCommandResult()
            {
                MetaData = new CqrsMessageResultMetaData(0, null, DateTime.Now, command.Guid)
            };

            await OnFail(exception,  command, commandResult);

            return commandResult;
        }

        protected TCommandResult Ok(TCommand command)
        {
            return new TCommandResult()
            {
                MetaData = new CqrsMessageResultMetaData(0, null, DateTime.Now, command.Guid)
            };
        }


        #region Template

        protected virtual Task<bool> ActivityAuthorizationIsConfirmed(TCommand command)
        {
            return Task.FromResult(true);
        }

        protected abstract Task Handle(TCommand command);

        protected virtual Task OnSucess(TCommand command, TCommandResult commandResult) { return Task.CompletedTask; }

        protected virtual Task OnFail(Exception exception, TCommand command, TCommandResult commandResult) { return Task.CompletedTask; }

        protected virtual Task PostExecutionValidate(TCommand command, TCommandResult commandResult) { return Task.CompletedTask; }

        protected virtual Task<TCommandResult> PreExecutionValidation(TCommand command)
        {
            return Task.FromResult(Ok(command));
        }

        #endregion

        #region Save Query and QeuryResult

        protected virtual void SaveCommand(TCommand command) { }

        protected virtual void SaveCommandResult(TCommandResult commandResult) { }

        #endregion
    }

}
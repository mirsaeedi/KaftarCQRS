﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Metadata;
using Kaftar.Core.Cqrs.CRUD;
using Kaftar.Core.Data;
using Kaftar.Core.EntityFramework;

namespace Kaftar.Core.Cqrs.CommandStack
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _context;

        public CommandDispatcher(IComponentContext context)
        {
            _context = context;
        }

        public async Task<CqrsCommandResult> Dispatch<TCommand>(TCommand command, int userId, string ip)
            where TCommand : CqrsCommand
        {
            command.IpAddress = ip;
            command.UserId = userId;

            var dataContext = _context.Resolve<IDataContext>();
            var setDataContext = _context.Resolve<ISetDataContext>();

            if (command.GetType().IsGenericType)
            {
                if (command.GetType().IsGenericType && command.GetType().GetGenericTypeDefinition() == typeof(CreateCqrsCommand<>))
                {
                    var handlerType = typeof(CreateCommandHandler<>);
                    return await HandlerCrudCommands(handlerType, command, dataContext);
                }

                if (command.GetType().IsGenericType && command.GetType().GetGenericTypeDefinition() == typeof(UpdateCqrsCommand<>))
                {
                    var handlerType = typeof(UpdateCqrsCommand<>);
                    return await HandlerCrudCommands(handlerType, command, dataContext);
                }

                if (command.GetType().IsGenericType && command.GetType().GetGenericTypeDefinition() == typeof(DeleteCqrsCommand<>))
                {
                    var handlerType = typeof(DeleteCqrsCommand<>);
                    return await HandlerCrudCommands(handlerType, command, dataContext);
                }
            }

            var handler =
                _context.Resolve<CommandHandler<TCommand, CqrsCommandResult>>();

            handler.DataContext = dataContext;
            handler.SetDataContext = setDataContext;

            return await handler.Execute(command);
        }

        private async Task<CqrsCommandResult> HandlerCrudCommands<TCommand>(Type handlerType, TCommand command, IDataContext dataContext)
        {
            Type[] typeArgs = { typeof(TCommand).GetGenericArguments()[0] };
            var genericHandlerType = handlerType.MakeGenericType(typeArgs);

            dynamic handler = Activator.CreateInstance(genericHandlerType);

            handler.DataContext = _context.Resolve<IDataContext>();
            handler.SetDataContext = _context.Resolve<ISetDataContext>();

            return await handler.Execute(command);
        }
    }
}
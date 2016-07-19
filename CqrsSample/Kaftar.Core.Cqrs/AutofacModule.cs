using System.Reflection;
using Autofac;
using CqrsSample.Core.CQRS.CommandStack;
using CqrsSample.Core.CQRS.CommandStack.CommandHandlers;
using CqrsSample.Core.CQRS.QueryStack;
using CqrsSample.Core.CQRS.QueryStack.QueryHandler;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.CommandStack.CommandHandlers;
using Kaftar.Core.Cqrs.QueryStack;
using Kaftar.Core.EntityFramework;
using Module = Autofac.Module;

namespace CqrsSample.Core
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
             .RegisterType<QueryDispatcher>()
             .InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(IQueryHandler<,>))
                   .PropertiesAutowired();

            builder
                .RegisterType<CommandDispatcher>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(ICommandHandler<,>))
                   .PropertiesAutowired();

            builder.RegisterType<DataContext>()
                .AsImplementedInterfaces();

            builder.RegisterType<ReadOnlyDataContext>()
                .AsImplementedInterfaces();

            builder.RegisterType<SetDataContext>()
                .AsImplementedInterfaces();
        }
    }
}

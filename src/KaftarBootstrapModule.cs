using System.Reflection;
using Autofac;
using Kaftar.Core.Cqrs.CommandStack;
using Kaftar.Core.Cqrs.QueryStack;
using Kaftar.Core.CQRS.QueryStack.QueryHandler;
using Kaftar.Core.Data;
using Kaftar.Core.EntityFramework;
using Module = Autofac.Module;

namespace Kaftar.Core.Cqrs
{
    public class KaftarBootstrapModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<QueryDispatcher>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CommandDispatcher>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<DataContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyDataContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<SetDataContext>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}

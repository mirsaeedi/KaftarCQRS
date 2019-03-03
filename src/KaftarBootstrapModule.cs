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
             .InstancePerRequest();

            builder
                .RegisterType<CommandDispatcher>()
                .InstancePerRequest();

            builder.RegisterType<DataContext>()
                .AsImplementedInterfaces();

            builder.RegisterType<ReadOnlyDataContext>()
                .AsImplementedInterfaces();

            builder.RegisterType<SetDataContext>()
                .AsImplementedInterfaces();
        }
    }
}

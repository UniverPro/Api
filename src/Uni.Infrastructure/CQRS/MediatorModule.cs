using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MediatR;
using Uni.Infrastructure.CQRS;
using Module = Autofac.Module;

namespace Uni.WebApi
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            var cqrsAssembly = typeof(CqrsMarker).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(cqrsAssembly)
                .AsImplementedInterfaces();

            // mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            var genericRequestHandlers = cqrsAssembly.ExportedTypes
                .Where(TypeIsGenericRequestHandler)
                .ToList();

            genericRequestHandlers.ForEach(x => builder.RegisterGeneric(x).AsImplementedInterfaces());

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }

        private static bool TypeIsGenericRequestHandler(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (!type.IsGenericTypeDefinition)
            {
                return false;
            }

            var interfaces = type.GetInterfaces();
            var genericInterfaces = interfaces.Where(i => i.IsGenericType);
            var typeDefinitionIsGenericRequestHandler = genericInterfaces.Any(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            return typeDefinitionIsGenericRequestHandler;
        }
    }
}

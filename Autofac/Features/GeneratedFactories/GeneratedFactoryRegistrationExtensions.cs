namespace Autofac.Features.GeneratedFactories
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using System;

    internal static class GeneratedFactoryRegistrationExtensions
    {
        internal static IRegistrationBuilder<TLimit, GeneratedFactoryActivatorData, SingleRegistrationStyle> RegisterGeneratedFactory<TLimit>(ContainerBuilder builder, Type delegateType, Service service)
        {
            GeneratedFactoryActivatorData activatorData = new GeneratedFactoryActivatorData(delegateType, service);
            RegistrationBuilder<TLimit, GeneratedFactoryActivatorData, SingleRegistrationStyle> rb = new RegistrationBuilder<TLimit, GeneratedFactoryActivatorData, SingleRegistrationStyle>(new TypedService(delegateType), activatorData, new SingleRegistrationStyle());
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                RegistrationBuilder.RegisterSingleComponent<TLimit, GeneratedFactoryActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb.InstancePerLifetimeScope();
        }
    }
}


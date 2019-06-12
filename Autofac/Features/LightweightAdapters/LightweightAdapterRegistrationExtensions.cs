namespace Autofac.Features.LightweightAdapters
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;

    internal static class LightweightAdapterRegistrationExtensions
    {
        public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo> adapter)
        {
            return RegisterAdapter<TFrom, TTo>(builder, adapter, new TypedService(typeof(TFrom)), new TypedService(typeof(TTo)));
        }

        private static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo> adapter, Service fromService, Service toService)
        {
            RegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> rb = new RegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle>(toService, new LightweightAdapterActivatorData(fromService, (c, p, f) => adapter(c, p, (TFrom) f)), new DynamicRegistrationStyle());
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                cr.AddRegistrationSource(new LightweightAdapterRegistrationSource(rb.RegistrationData, rb.ActivatorData));
            });
            return rb;
        }

        public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TService, TService> decorator, object fromKey, object toKey)
        {
            return RegisterAdapter<TService, TService>(builder, decorator, ServiceWithKey<TService>(fromKey), ServiceWithKey<TService>(toKey));
        }

        private static Service ServiceWithKey<TService>(object key)
        {
            if (key == null)
            {
                return new TypedService(typeof(TService));
            }
            return new KeyedService(key, typeof(TService));
        }
    }
}


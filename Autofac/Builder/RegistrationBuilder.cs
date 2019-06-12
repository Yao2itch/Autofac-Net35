namespace Autofac.Builder
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Core.Registration;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class RegistrationBuilder
    {
        public static IComponentRegistration CreateRegistration<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> rb) where TActivatorData: IConcreteActivatorData where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            return CreateRegistration(rb.RegistrationStyle.Id, rb.RegistrationData, rb.ActivatorData.Activator, rb.RegistrationData.Services, rb.RegistrationStyle.Target);
        }

        public static IComponentRegistration CreateRegistration(Guid id, RegistrationData data, IInstanceActivator activator, IEnumerable<Service> services)
        {
            return CreateRegistration(id, data, activator, services, null);
        }

        public static IComponentRegistration CreateRegistration(Guid id, RegistrationData data, IInstanceActivator activator, IEnumerable<Service> services, IComponentRegistration target)
        {
            IComponentRegistration registration;
            Type limitType = activator.LimitType;
            if (limitType != typeof(object))
            {
                foreach (IServiceWithType type2 in services.OfType<IServiceWithType>())
                {
                    if (!type2.ServiceType.IsAssignableFrom(limitType))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, RegistrationBuilderResources.ComponentDoesNotSupportService, new object[] { limitType, type2 }));
                    }
                }
            }
            if (target == null)
            {
                registration = new ComponentRegistration(id, activator, data.Lifetime, data.Sharing, data.Ownership, services, data.Metadata);
            }
            else
            {
                registration = new ComponentRegistration(id, activator, data.Lifetime, data.Sharing, data.Ownership, services, data.Metadata, target);
            }
            foreach (EventHandler<PreparingEventArgs> handler in data.PreparingHandlers)
            {
                registration.Preparing += handler;
            }
            foreach (EventHandler<ActivatingEventArgs<object>> handler2 in data.ActivatingHandlers)
            {
                registration.Activating += handler2;
            }
            foreach (EventHandler<ActivatedEventArgs<object>> handler3 in data.ActivatedHandlers)
            {
                registration.Activated += handler3;
            }
            return registration;
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> ForDelegate<T>(Func<IComponentContext, IEnumerable<Parameter>, T> @delegate)
        {
            return new RegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle>(new TypedService(typeof(T)), new SimpleActivatorData(new DelegateActivator(typeof(T), (c, p) => @delegate(c, p))), new SingleRegistrationStyle());
        }

        public static IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> ForDelegate(Type limitType, Func<IComponentContext, IEnumerable<Parameter>, object> @delegate)
        {
            return new RegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle>(new TypedService(limitType), new SimpleActivatorData(new DelegateActivator(limitType, @delegate)), new SingleRegistrationStyle());
        }

        public static IRegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle> ForType<TImplementor>()
        {
            return new RegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new TypedService(typeof(TImplementor)), new ConcreteReflectionActivatorData(typeof(TImplementor)), new SingleRegistrationStyle());
        }

        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> ForType(Type implementationType)
        {
            return new RegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(new TypedService(implementationType), new ConcreteReflectionActivatorData(implementationType), new SingleRegistrationStyle());
        }

        public static void RegisterSingleComponent<TLimit, TActivatorData, TSingleRegistrationStyle>(IComponentRegistry cr, IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> rb) where TActivatorData: IConcreteActivatorData where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            IComponentRegistration registration = rb.CreateRegistration<TLimit, TActivatorData, TSingleRegistrationStyle>();
            cr.Register(registration, rb.RegistrationStyle.PreserveDefaults);
            ComponentRegisteredEventArgs e = new ComponentRegisteredEventArgs(cr, registration);
            foreach (EventHandler<ComponentRegisteredEventArgs> handler in rb.RegistrationStyle.RegisteredHandlers)
            {
                handler(cr, e);
            }
        }
    }
}


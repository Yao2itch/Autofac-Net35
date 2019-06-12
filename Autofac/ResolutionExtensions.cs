namespace Autofac
{
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using Autofac.Core.Registration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class ResolutionExtensions
    {
        private static readonly IEnumerable<Parameter> NoParameters = Enumerable.Empty<Parameter>();

        public static TService InjectProperties<TService>(this IComponentContext context, TService instance)
        {
            new AutowiringPropertyInjector().InjectProperties(context, instance, true);
            return instance;
        }

        public static TService InjectUnsetProperties<TService>(this IComponentContext context, TService instance)
        {
            new AutowiringPropertyInjector().InjectProperties(context, instance, false);
            return instance;
        }

        public static bool IsRegistered<TService>(this IComponentContext context)
        {
            return context.IsRegistered(typeof(TService));
        }

        public static bool IsRegistered(this IComponentContext context, Type serviceType)
        {
            return context.IsRegisteredService(new TypedService(serviceType));
        }

        public static bool IsRegisteredService(this IComponentContext context, Service service)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            return context.ComponentRegistry.IsRegistered(service);
        }

        public static bool IsRegisteredWithKey<TService>(this IComponentContext context, object serviceKey)
        {
            return context.IsRegisteredWithKey(serviceKey, typeof(TService));
        }

        public static bool IsRegisteredWithKey(this IComponentContext context, object serviceKey, Type serviceType)
        {
            return context.IsRegisteredService(new KeyedService(serviceKey, serviceType));
        }

        public static bool IsRegisteredWithName<TService>(this IComponentContext context, string serviceName)
        {
            return context.IsRegisteredWithKey<TService>(serviceName);
        }

        public static bool IsRegisteredWithName(this IComponentContext context, string serviceName, Type serviceType)
        {
            return context.IsRegisteredWithKey(serviceName, serviceType);
        }

        public static TService Resolve<TService>(this IComponentContext context)
        {
            return context.Resolve<TService>(NoParameters);
        }

        public static TService Resolve<TService>(this IComponentContext context, IEnumerable<Parameter> parameters)
        {
            return (TService) context.Resolve(typeof(TService), parameters);
        }

        public static object Resolve(this IComponentContext context, Type serviceType)
        {
            return context.Resolve(serviceType, NoParameters);
        }

        public static TService Resolve<TService>(this IComponentContext context, params Parameter[] parameters)
        {
            return context.Resolve<TService>(((IEnumerable<Parameter>) parameters));
        }

        public static object Resolve(this IComponentContext context, Type serviceType, IEnumerable<Parameter> parameters)
        {
            return context.ResolveService(new TypedService(serviceType), parameters);
        }

        public static object Resolve(this IComponentContext context, Type serviceType, params Parameter[] parameters)
        {
            return context.Resolve(serviceType, ((IEnumerable<Parameter>) parameters));
        }

        public static TService ResolveKeyed<TService>(this IComponentContext context, object serviceKey)
        {
            return context.ResolveKeyed<TService>(serviceKey, NoParameters);
        }

        public static TService ResolveKeyed<TService>(this IComponentContext context, object serviceKey, IEnumerable<Parameter> parameters)
        {
            return (TService) context.ResolveService(new KeyedService(serviceKey, typeof(TService)), parameters);
        }

        public static TService ResolveKeyed<TService>(this IComponentContext context, object serviceKey, params Parameter[] parameters)
        {
            return context.ResolveKeyed<TService>(serviceKey, ((IEnumerable<Parameter>) parameters));
        }

        public static TService ResolveNamed<TService>(this IComponentContext context, string serviceName)
        {
            return context.ResolveNamed<TService>(serviceName, NoParameters);
        }

        public static TService ResolveNamed<TService>(this IComponentContext context, string serviceName, IEnumerable<Parameter> parameters)
        {
            return (TService) context.ResolveService(new KeyedService(serviceName, typeof(TService)), parameters);
        }

        public static object ResolveNamed(this IComponentContext context, string serviceName, Type serviceType)
        {
            return context.ResolveNamed(serviceName, serviceType, NoParameters);
        }

        public static TService ResolveNamed<TService>(this IComponentContext context, string serviceName, params Parameter[] parameters)
        {
            return context.ResolveNamed<TService>(serviceName, ((IEnumerable<Parameter>) parameters));
        }

        public static object ResolveNamed(this IComponentContext context, string serviceName, Type serviceType, IEnumerable<Parameter> parameters)
        {
            return context.ResolveService(new KeyedService(serviceName, serviceType), parameters);
        }

        public static object ResolveNamed(this IComponentContext context, string serviceName, Type serviceType, params Parameter[] parameters)
        {
            return context.ResolveNamed(serviceName, serviceType, ((IEnumerable<Parameter>) parameters));
        }

        public static TService ResolveOptional<TService>(this IComponentContext context) where TService: class
        {
            return context.ResolveOptional<TService>(NoParameters);
        }

        public static TService ResolveOptional<TService>(this IComponentContext context, IEnumerable<Parameter> parameters) where TService: class
        {
            return (TService) context.ResolveOptionalService(new TypedService(typeof(TService)), parameters);
        }

        public static object ResolveOptional(this IComponentContext context, Type serviceType)
        {
            return context.ResolveOptional(serviceType, NoParameters);
        }

        public static TService ResolveOptional<TService>(this IComponentContext context, params Parameter[] parameters) where TService: class
        {
            return context.ResolveOptional<TService>(((IEnumerable<Parameter>) parameters));
        }

        public static object ResolveOptional(this IComponentContext context, Type serviceType, IEnumerable<Parameter> parameters)
        {
            return context.ResolveOptionalService(new TypedService(serviceType), parameters);
        }

        public static object ResolveOptional(this IComponentContext context, Type serviceType, params Parameter[] parameters)
        {
            return context.ResolveOptional(serviceType, ((IEnumerable<Parameter>) parameters));
        }

        public static TService ResolveOptionalKeyed<TService>(this IComponentContext context, object serviceKey) where TService: class
        {
            return context.ResolveOptionalKeyed<TService>(serviceKey, NoParameters);
        }

        public static TService ResolveOptionalKeyed<TService>(this IComponentContext context, object serviceKey, IEnumerable<Parameter> parameters) where TService: class
        {
            return (TService) context.ResolveOptionalService(new KeyedService(serviceKey, typeof(TService)), parameters);
        }

        public static TService ResolveOptionalKeyed<TService>(this IComponentContext context, object serviceKey, params Parameter[] parameters) where TService: class
        {
            return context.ResolveOptionalKeyed<TService>(serviceKey, ((IEnumerable<Parameter>) parameters));
        }

        public static TService ResolveOptionalNamed<TService>(this IComponentContext context, string serviceName) where TService: class
        {
            return context.ResolveOptionalKeyed<TService>(serviceName);
        }

        public static TService ResolveOptionalNamed<TService>(this IComponentContext context, string serviceName, IEnumerable<Parameter> parameters) where TService: class
        {
            return context.ResolveOptionalKeyed<TService>(serviceName, parameters);
        }

        public static TService ResolveOptionalNamed<TService>(this IComponentContext context, string serviceName, params Parameter[] parameters) where TService: class
        {
            return context.ResolveOptionalKeyed<TService>(serviceName, parameters);
        }

        public static object ResolveOptionalService(this IComponentContext context, Service service)
        {
            return context.ResolveOptionalService(service, NoParameters);
        }

        public static object ResolveOptionalService(this IComponentContext context, Service service, IEnumerable<Parameter> parameters)
        {
            object obj2;
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            context.TryResolveService(service, parameters, out obj2);
            return obj2;
        }

        public static object ResolveOptionalService(this IComponentContext context, Service service, params Parameter[] parameters)
        {
            return context.ResolveOptionalService(service, ((IEnumerable<Parameter>) parameters));
        }

        public static object ResolveService(this IComponentContext context, Service service)
        {
            return context.ResolveService(service, NoParameters);
        }

        public static object ResolveService(this IComponentContext context, Service service, IEnumerable<Parameter> parameters)
        {
            object obj2;
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (!context.TryResolveService(service, parameters, out obj2))
            {
                throw new ComponentNotRegisteredException(service);
            }
            return obj2;
        }

        public static object ResolveService(this IComponentContext context, Service service, params Parameter[] parameters)
        {
            return context.ResolveService(service, ((IEnumerable<Parameter>) parameters));
        }

        public static bool TryResolve<T>(this IComponentContext context, out T instance)
        {
            object obj2;
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            bool flag = context.TryResolve(typeof(T), out obj2);
            instance = flag ? ((T) obj2) : default(T);
            return flag;
        }

        public static bool TryResolve(this IComponentContext context, Type serviceType, out object instance)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.TryResolveService(new TypedService(serviceType), NoParameters, out instance);
        }

        public static bool TryResolveKeyed(this IComponentContext context, object serviceKey, Type serviceType, out object instance)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.TryResolveService(new KeyedService(serviceKey, serviceType), NoParameters, out instance);
        }

        public static bool TryResolveNamed(this IComponentContext context, string serviceName, Type serviceType, out object instance)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.TryResolveService(new KeyedService(serviceName, serviceType), NoParameters, out instance);
        }

        public static bool TryResolveService(this IComponentContext context, Service service, out object instance)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.TryResolveService(service, NoParameters, out instance);
        }

        public static bool TryResolveService(this IComponentContext context, Service service, IEnumerable<Parameter> parameters, out object instance)
        {
            IComponentRegistration registration;
            if (!context.ComponentRegistry.TryGetRegistration(service, out registration))
            {
                instance = null;
                return false;
            }
            instance = context.ResolveComponent(registration, parameters);
            return true;
        }
    }
}


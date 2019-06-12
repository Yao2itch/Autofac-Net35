namespace Autofac
{
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Core.Activators.ProvidedInstance;
    using Autofac.Core.Activators.Reflection;
    using Autofac.Core.Lifetime;
    using Autofac.Features.LightweightAdapters;
    using Autofac.Features.OpenGenerics;
    using Autofac.Features.Scanning;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class RegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, Service> serviceMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As<TLimit, TScanningActivatorData, TRegistrationStyle>(((Func<Type, IEnumerable<Service>>) (t => new Service[] { serviceMapping(t) })));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<Service>> serviceMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (serviceMapping == null)
            {
                throw new ArgumentNullException("serviceMapping");
            }
            return ScanningRegistrationExtensions.As<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, serviceMapping);
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<Type>> serviceMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As<TLimit, TScanningActivatorData, TRegistrationStyle>(((Func<Type, IEnumerable<Service>>) (t => (from s in serviceMapping(t) select (Service)new TypedService(s)))));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, Type> serviceMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As<TLimit, TScanningActivatorData, TRegistrationStyle>(t => new TypedService(serviceMapping(t)));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type openGenericServiceType) where TScanningActivatorData: ScanningActivatorData
        {
            return ScanningRegistrationExtensions.AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(registration, openGenericServiceType);
        }

        public static IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> AsImplementedInterfaces<TLimit>(this IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            Type implementationType = registration.ActivatorData.ImplementationType;
            return registration.As(GetImplementedInterfaces(implementationType));
        }

        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> AsImplementedInterfaces<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As<TLimit, ScanningActivatorData, DynamicRegistrationStyle>(((Func<Type, IEnumerable<Type>>) (t => GetImplementedInterfaces(t))));
        }

        public static IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> AsImplementedInterfaces<TLimit, TConcreteActivatorData>(this IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> registration) where TConcreteActivatorData: IConcreteActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As(GetImplementedInterfaces(registration.ActivatorData.Activator.LimitType));
        }

        public static IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> AsSelf<TLimit>(this IRegistrationBuilder<TLimit, ReflectionActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As(new Type[] { registration.ActivatorData.ImplementationType });
        }

        public static IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> AsSelf<TLimit>(this IRegistrationBuilder<TLimit, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As<TLimit, ScanningActivatorData, DynamicRegistrationStyle>(t => t);
        }

        public static IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> AsSelf<TLimit, TConcreteActivatorData>(this IRegistrationBuilder<TLimit, TConcreteActivatorData, SingleRegistrationStyle> registration) where TConcreteActivatorData: IConcreteActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.As(new Type[] { registration.ActivatorData.Activator.LimitType });
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> AssignableTo<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            return registration.AssignableTo<object, ScanningActivatorData, DynamicRegistrationStyle>(typeof(T));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type type) where TScanningActivatorData: ScanningActivatorData
        {
            return registration.AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(type);
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Except<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            return registration.Where<object, ScanningActivatorData, DynamicRegistrationStyle>(t => (t != typeof(T)));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Except<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Action<IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>> customisedRegistration)
        {
            var result = registration.Except<T>();

            result.ActivatorData.PostScanningCallbacks.Add(cr =>
            {
                var rb = RegistrationBuilder.ForType<T>();
                customisedRegistration(rb);
                RegistrationBuilder.RegisterSingleComponent(cr, rb);
            });

            return result;

            //IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builder = registration.Except<T>();
            //builder.ActivatorData.PostScanningCallbacks.Add(delegate (IComponentRegistry cr) {
            //    IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder = RegistrationBuilder.ForType<T>();
            //    customisedRegistration(builder);
            //    RegistrationBuilder.RegisterSingleComponent<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, builder);
            //});
            //return builder;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IConstructorFinder constructorFinder) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (constructorFinder == null)
            {
                throw new ArgumentNullException("constructorFinder");
            }
            registration.ActivatorData.ConstructorFinder = constructorFinder;
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, BindingFlags bindingFlags) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.FindConstructorsWith<TLimit, TReflectionActivatorData, TStyle>(new BindingFlagsConstructorFinder(bindingFlags));
        }

        private static Type[] GetImplementedInterfaces(Type type)
        {
            return (from i in type.GetInterfaces()
                where i != typeof(IDisposable)
                select i).ToArray<Type>();
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> InNamespace<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, string ns) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (ns == null)
            {
                throw new ArgumentNullException("ns");
            }
            return registration.Where<TLimit, TScanningActivatorData, TRegistrationStyle>(t => t.IsInNamespace(ns));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> InNamespaceOf<T>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.InNamespace<object, ScanningActivatorData, DynamicRegistrationStyle>(typeof(T).Namespace);
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Keyed<TService>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Func<Type, object> serviceKeyMapping)
        {
            return registration.Keyed<object, ScanningActivatorData, DynamicRegistrationStyle>(serviceKeyMapping, typeof(TService));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Keyed<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, object> serviceKeyMapping, Type serviceType) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (serviceKeyMapping == null)
            {
                throw new ArgumentNullException("serviceKeyMapping");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return registration.AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(serviceType).As<TLimit, TScanningActivatorData, TRegistrationStyle>(t => new KeyedService(serviceKeyMapping(t), serviceType));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> Named<TService>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration, Func<Type, string> serviceNameMapping)
        {
            return registration.Named<object, ScanningActivatorData, DynamicRegistrationStyle>(serviceNameMapping, typeof(TService));
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Named<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, string> serviceNameMapping, Type serviceType) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (serviceNameMapping == null)
            {
                throw new ArgumentNullException("serviceNameMapping");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return registration.As<TLimit, TScanningActivatorData, TRegistrationStyle>(t => new KeyedService(serviceNameMapping(t), serviceType));
        }

        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> OnRegistered<TLimit, TRegistrationStyle>(this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration, Action<ComponentRegisteredEventArgs> handler)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            registration.ActivatorData.ConfigurationActions.Add(delegate (Type t, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb) {
                rb.OnRegistered<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(handler);
            });
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> OnRegistered<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, Action<ComponentRegisteredEventArgs> handler) where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            registration.RegistrationStyle.RegisteredHandlers.Add(delegate (object s, ComponentRegisteredEventArgs e) {
                handler(e);
            });
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnRelease<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, Action<TLimit> releaseAction)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (releaseAction == null)
            {
                throw new ArgumentNullException("releaseAction");
            }
            return registration.ExternallyOwned().OnActivating(delegate (IActivatingEventArgs<TLimit> e) {
                ReleaseAction instance = new ReleaseAction(() => releaseAction(e.Instance));
                e.Context.Resolve<ILifetimeScope>().Disposer.AddInstanceForDisposal(instance);
            });
        }

        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> PreserveExistingDefaults<TLimit, TRegistrationStyle>(this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
        {
            return ScanningRegistrationExtensions.PreserveExistingDefaults<TLimit, ScanningActivatorData, TRegistrationStyle>(registration);
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> PreserveExistingDefaults<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration) where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            registration.RegistrationStyle.PreserveDefaults = true;
            return registration;
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(this ContainerBuilder builder, Func<IComponentContext, T> @delegate)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }
            return builder.Register<T>((c, p) => @delegate(c));
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, T> @delegate)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }
            IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForDelegate<T>(@delegate);
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                RegistrationBuilder.RegisterSingleComponent<T, SimpleActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb;
        }

        public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<TFrom, TTo> adapter)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }
            return builder.RegisterAdapter<TFrom, TTo>((c, p, f) => adapter(f));
        }

        public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<IComponentContext, TFrom, TTo> adapter)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }
            return builder.RegisterAdapter<TFrom, TTo>((c, p, f) => adapter(c, f));
        }

        public static IRegistrationBuilder<TTo, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterAdapter<TFrom, TTo>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TFrom, TTo> adapter)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }
            return LightweightAdapterRegistrationExtensions.RegisterAdapter<TFrom, TTo>(builder, adapter);
        }

        public static void RegisterAssemblyModules(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterAssemblyModules<IModule>(assemblies);
        }

        public static void RegisterAssemblyModules<TModule>(this ContainerBuilder builder, params Assembly[] assemblies) where TModule: IModule
        {
            ContainerBuilder builder2 = new ContainerBuilder();
            Type type = typeof(TModule);
            builder2.RegisterAssemblyTypes(assemblies).Where<object, ScanningActivatorData, DynamicRegistrationStyle>(new Func<Type, bool>(type.IsAssignableFrom)).As<IModule>();
            using (IContainer container = builder2.Build(ContainerBuildOptions.Default))
            {
                foreach (IModule module in container.Resolve<IEnumerable<IModule>>())
                {
                    builder.RegisterModule(module);
                }
            }
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            return ScanningRegistrationExtensions.RegisterAssemblyTypes(builder, assemblies);
        }

        public static void RegisterComponent(this ContainerBuilder builder, IComponentRegistration registration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            builder.RegisterCallback(cr => cr.Register(registration));
        }

        public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<TService, TService> decorator, object fromKey, object toKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (decorator == null)
            {
                throw new ArgumentNullException("decorator");
            }
            return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, (c, p, f) => decorator(f), fromKey, toKey);
        }

        public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<IComponentContext, TService, TService> decorator, object fromKey, object toKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (decorator == null)
            {
                throw new ArgumentNullException("decorator");
            }
            return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, (c, p, f) => decorator(c, f), fromKey, toKey);
        }

        public static IRegistrationBuilder<TService, LightweightAdapterActivatorData, DynamicRegistrationStyle> RegisterDecorator<TService>(this ContainerBuilder builder, Func<IComponentContext, IEnumerable<Parameter>, TService, TService> decorator, object fromKey, object toKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (decorator == null)
            {
                throw new ArgumentNullException("decorator");
            }
            return LightweightAdapterRegistrationExtensions.RegisterDecorator<TService>(builder, decorator, fromKey, toKey);
        }

        public static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> RegisterGeneric(this ContainerBuilder builder, Type implementor)
        {
            return OpenGenericRegistrationExtensions.RegisterGeneric(builder, implementor);
        }

        public static IRegistrationBuilder<object, OpenGenericDecoratorActivatorData, DynamicRegistrationStyle> RegisterGenericDecorator(this ContainerBuilder builder, Type decoratorType, Type decoratedServiceType, object fromKey, object toKey)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (decoratorType == null)
            {
                throw new ArgumentNullException("decoratorType");
            }
            if (decoratedServiceType == null)
            {
                throw new ArgumentNullException("decoratedServiceType");
            }
            return OpenGenericRegistrationExtensions.RegisterGenericDecorator(builder, decoratorType, decoratedServiceType, fromKey, toKey);
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstance<T>(this ContainerBuilder builder, T instance) where T: class
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            ProvidedInstanceActivator activator = new ProvidedInstanceActivator(instance);
            RegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> rb = new RegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle>(new TypedService(typeof(T)), new SimpleActivatorData(activator), new SingleRegistrationStyle());
            rb.SingleInstance();
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                if (!(rb.RegistrationData.Lifetime is RootScopeLifetime) || (rb.RegistrationData.Sharing != InstanceSharing.Shared))
                {
                    throw new InvalidOperationException(string.Format(RegistrationExtensionsResources.InstanceRegistrationsAreSingleInstanceOnly, instance));
                }
                activator.DisposeInstance = rb.RegistrationData.Ownership == InstanceOwnership.OwnedByLifetimeScope;
                RegistrationBuilder.RegisterSingleComponent<T, SimpleActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb;
        }

        public static void RegisterModule<TModule>(this ContainerBuilder builder) where TModule: IModule, new()
        {
            builder.RegisterModule((default(TModule) == null) ? ((IModule) Activator.CreateInstance<TModule>()) : ((IModule) default(TModule)));
        }

        public static void RegisterModule(this ContainerBuilder builder, IModule module)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            builder.RegisterCallback(new Action<IComponentRegistry>(module.Configure));
        }

        public static void RegisterSource(this ContainerBuilder builder, IRegistrationSource registrationSource)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (registrationSource == null)
            {
                throw new ArgumentNullException("registrationSource");
            }
            builder.RegisterCallback(cr => cr.AddRegistrationSource(registrationSource));
        }

        public static IRegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<TImplementor>(this ContainerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            IRegistrationBuilder<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForType<TImplementor>();
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                RegistrationBuilder.RegisterSingleComponent<TImplementor, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb;
        }

        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(this ContainerBuilder builder, Type implementationType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (implementationType == null)
            {
                throw new ArgumentNullException("implementationType");
            }
            IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForType(implementationType);
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                RegistrationBuilder.RegisterSingleComponent<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> Targeting<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, IComponentRegistration target) where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            registration.RegistrationStyle.Target = target.Target;
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, params Type[] signature) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            if (registration.ActivatorData.ImplementationType.GetConstructor(signature) == null)
            {
                throw new ArgumentException(string.Format(RegistrationExtensionsResources.NoMatchingConstructorExists, registration.ActivatorData.ImplementationType));
            }
            return registration.UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(new MatchingSignatureConstructorSelector(signature));
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> UsingConstructor<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IConstructorSelector constructorSelector) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (constructorSelector == null)
            {
                throw new ArgumentNullException("constructorSelector");
            }
            registration.ActivatorData.ConstructorSelector = constructorSelector;
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> Where<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, bool> predicate) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            registration.ActivatorData.Filters.Add(predicate);
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<KeyValuePair<string, object>>> metadataMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            registration.ActivatorData.ConfigurationActions.Add(delegate (Type t, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb) {
                rb.WithMetadata(metadataMapping(t));
            });
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, string metadataKey, Func<Type, object> metadataValueMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            return registration.WithMetadata<TLimit, TScanningActivatorData, TRegistrationStyle>(((Func<Type, IEnumerable<KeyValuePair<string, object>>>) (t => new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(metadataKey, metadataValueMapping(t)) })));
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> WithMetadataFrom<TAttribute>(this IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registration)
        {
            Type type = typeof(TAttribute);
            IEnumerable<PropertyInfo> metadataProperties = from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where pi.CanRead
                select pi;
            return registration.WithMetadata<object, ScanningActivatorData, DynamicRegistrationStyle>(delegate (Type t) {
                TAttribute[] localArray = t.GetCustomAttributes(true).OfType<TAttribute>().ToArray<TAttribute>();
                if (localArray.Length == 0)
                {
                    throw new ArgumentException(string.Format("A metadata attribute of type {0} was not found on {1}.", typeof(TAttribute), t));
                }
                if (localArray.Length != 1)
                {
                    throw new ArgumentException(string.Format("More than one metadata attribute of type {0} was found on {1}.", typeof(TAttribute), t));
                }
                TAttribute attr = localArray[0];
                return (from p in metadataProperties select new KeyValuePair<string, object>(p.Name, p.GetValue(attr, null)));
            });
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Parameter parameter) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }
            registration.ActivatorData.ConfiguredParameters.Add(parameter);
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Func<ParameterInfo, IComponentContext, bool> parameterSelector, Func<ParameterInfo, IComponentContext, object> valueProvider) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (parameterSelector == null)
            {
                throw new ArgumentNullException("parameterSelector");
            }
            if (valueProvider == null)
            {
                throw new ArgumentNullException("valueProvider");
            }
            return registration.WithParameter<TLimit, TReflectionActivatorData, TStyle>(new ResolvedParameter(parameterSelector, valueProvider));
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameter<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, string parameterName, object parameterValue) where TReflectionActivatorData: ReflectionActivatorData
        {
            return registration.WithParameter<TLimit, TReflectionActivatorData, TStyle>(new NamedParameter(parameterName, parameterValue));
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithParameters<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IEnumerable<Parameter> parameters) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            foreach (Parameter parameter in parameters)
            {
                registration.WithParameter<TLimit, TReflectionActivatorData, TStyle>(parameter);
            }
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperties<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, IEnumerable<Parameter> properties) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            foreach (Parameter parameter in properties)
            {
                registration.WithProperty<TLimit, TReflectionActivatorData, TStyle>(parameter);
            }
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperty<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, Parameter property) where TReflectionActivatorData: ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            registration.ActivatorData.ConfiguredProperties.Add(property);
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithProperty<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration, string propertyName, object propertyValue) where TReflectionActivatorData: ReflectionActivatorData
        {
            return registration.WithProperty<TLimit, TReflectionActivatorData, TStyle>(new NamedPropertyParameter(propertyName, propertyValue));
        }
    }
}


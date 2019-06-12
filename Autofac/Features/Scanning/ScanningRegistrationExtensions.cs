namespace Autofac.Features.Scanning
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class ScanningRegistrationExtensions
    {
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> As<TLimit, TScanningActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Func<Type, IEnumerable<Service>> serviceMapping) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (serviceMapping == null)
            {
                throw new ArgumentNullException("serviceMapping");
            }
            registration.ActivatorData.ConfigurationActions.Add(delegate (Type t, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> rb) {
                IEnumerable<Service> source = serviceMapping(t);
                Type impl = rb.ActivatorData.ImplementationType;
                IEnumerable<Service> enumerable2 = source.Where<Service>(delegate (Service s) {
                    if (s is IServiceWithType)
                    {
                        return ((IServiceWithType) s).ServiceType.IsAssignableFrom(impl);
                    }
                    return true;
                });
                rb.As(enumerable2.ToArray<Service>());
            });
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle>
            AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(
                IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration,
                Type openGenericServiceType)
            where TScanningActivatorData : ScanningActivatorData
        {
            if (openGenericServiceType == null) throw new ArgumentNullException("openGenericServiceType");

            return registration
                .Where(candidateType => candidateType.IsClosedTypeOf(openGenericServiceType))
                .As(candidateType => candidateType.GetTypesThatClose(openGenericServiceType)
                        .Select(t => (Service)new TypedService(t)));
        }

        //public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AsClosedTypesOf<TLimit, TScanningActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type openGenericServiceType) where TScanningActivatorData: ScanningActivatorData
        //{
        //    if (openGenericServiceType == null)
        //    {
        //        throw new ArgumentNullException("openGenericServiceType");
        //    }
        //    return registration.Where<TLimit, TScanningActivatorData, TRegistrationStyle>(candidateType => candidateType.IsClosedTypeOf(openGenericServiceType)).As<TLimit, TScanningActivatorData, TRegistrationStyle>(((Func<Type, IEnumerable<Service>>) (candidateType => (from t in candidateType.GetTypesThatClose(openGenericServiceType) select new TypedService(t)))));
        //}

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> AssignableTo<TLimit, TScanningActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration, Type type) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            registration.ActivatorData.Filters.Add(new Func<Type, bool>(type.IsAssignableFrom));
            return registration;
        }

        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> PreserveExistingDefaults<TLimit, TScanningActivatorData, TRegistrationStyle>(IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration) where TScanningActivatorData: ScanningActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            registration.ActivatorData.ConfigurationActions.Add(delegate (Type t, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> r) {
                r.PreserveExistingDefaults<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>();
            });
            return registration;
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(ContainerBuilder builder, params Assembly[] assemblies)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (assemblies == null)
            {
                throw new ArgumentNullException("assemblies");
            }
            RegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> rb = new RegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>(new TypedService(typeof(object)), new ScanningActivatorData(), new DynamicRegistrationStyle());
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                ScanAssemblies(assemblies, cr, rb);
            });
            return rb;
        }

        private static void ScanAssemblies(IEnumerable<Assembly> assemblies, IComponentRegistry cr, IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> rb)
        {
            ScanTypes(assemblies.SelectMany(a => a.GetLoadableTypes()), cr, rb);
            //rb.ActivatorData.Filters.Add(t => rb.RegistrationData.Services.OfType<IServiceWithType>().All<IServiceWithType>(swt => swt.ServiceType.IsAssignableFrom(t)));
            //foreach (Type type in from a in assemblies
            //    select a.GetTypes() into t
            //    where ((t.IsClass && !t.IsAbstract) && (!t.IsGenericTypeDefinition && !t.IsDelegate())) && rb.ActivatorData.Filters.All<Func<Type, bool>>(p => p(t))
            //    select t)
            //{
            //    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> builder = RegistrationBuilder.ForType(type).FindConstructorsWith<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(rb.ActivatorData.ConstructorFinder).UsingConstructor<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(rb.ActivatorData.ConstructorSelector).WithParameters<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(rb.ActivatorData.ConfiguredParameters).WithProperties<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(rb.ActivatorData.ConfiguredProperties);
            //    builder.RegistrationData.CopyFrom(rb.RegistrationData, false);
            //    foreach (Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> action in rb.ActivatorData.ConfigurationActions)
            //    {
            //        action(type, builder);
            //    }
            //    if (builder.RegistrationData.Services.Any<Service>())
            //    {
            //        RegistrationBuilder.RegisterSingleComponent<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(cr, builder);
            //    }
            //}
            //foreach (Action<IComponentRegistry> action2 in rb.ActivatorData.PostScanningCallbacks)
            //{
            //    action2(cr);
            //}
        }

        static void ScanTypes(IEnumerable<Type> types, IComponentRegistry cr, IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> rb)
        {
            rb.ActivatorData.Filters.Add(t =>
                rb.RegistrationData.Services.OfType<IServiceWithType>().All(swt =>
                    swt.ServiceType.IsAssignableFrom(t)));

            foreach (var t in types
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericTypeDefinition &&
                    !t.IsDelegate() &&
                    rb.ActivatorData.Filters.All(p => p(t))))
            {
                var scanned = RegistrationBuilder.ForType(t)
                    .FindConstructorsWith(rb.ActivatorData.ConstructorFinder)
                    .UsingConstructor(rb.ActivatorData.ConstructorSelector)
                    .WithParameters(rb.ActivatorData.ConfiguredParameters)
                    .WithProperties(rb.ActivatorData.ConfiguredProperties);

                scanned.RegistrationData.CopyFrom(rb.RegistrationData, false);

                foreach (var action in rb.ActivatorData.ConfigurationActions)
                    action(t, scanned);

                if (scanned.RegistrationData.Services.Any())
                    RegistrationBuilder.RegisterSingleComponent(cr, scanned);
            }

            foreach (var postScanningCallback in rb.ActivatorData.PostScanningCallbacks)
                postScanningCallback(cr);
        }
    }
}


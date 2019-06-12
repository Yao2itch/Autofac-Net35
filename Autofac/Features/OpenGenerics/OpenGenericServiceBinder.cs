namespace Autofac.Features.OpenGenerics
{
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    internal static class OpenGenericServiceBinder
    {
        public static void EnforceBindable(Type implementationType, IEnumerable<Service> services)
        {
            if (implementationType == null)
            {
                throw new ArgumentNullException("implementationType");
            }
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (!implementationType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(string.Format(OpenGenericServiceBinderResources.ImplementorMustBeOpenGenericTypeDefinition, implementationType));
            }
            using (IEnumerator<Service> enumerator = services.GetEnumerator())
            {
                Func<Type, bool> predicate = null;
                while (enumerator.MoveNext())
                {
                    IServiceWithType service = (IServiceWithType) enumerator.Current;
                    if (!service.ServiceType.IsGenericTypeDefinition)
                    {
                        throw new ArgumentException(string.Format(OpenGenericServiceBinderResources.ServiceTypeMustBeOpenGenericTypeDefinition, service));
                    }
                    if (service.ServiceType.IsInterface)
                    {
                        if (GetInterface(implementationType, service.ServiceType) == null)
                        {
                            throw new ArgumentException(string.Format(OpenGenericServiceBinderResources.InterfaceIsNotImplemented, implementationType, service));
                        }
                    }
                    else
                    {
                        if (predicate == null)
                        {
                            predicate = t => IsCompatibleGenericClassDefinition(t, service.ServiceType);
                        }
                        if (!Traverse.Across<Type>(implementationType, t => t.BaseType).Any<Type>(predicate))
                        {
                            throw new ArgumentException(string.Format(OpenGenericServiceBinderResources.TypesAreNotConvertible, implementationType, service));
                        }
                    }
                }
            }
        }

        private static Type GetInterface(Type implementationType, Type serviceType)
        {
            return implementationType.GetInterface(serviceType.Name);
        }

        private static bool IsCompatibleGenericClassDefinition(Type implementor, Type serviceType)
        {
            return ((implementor == serviceType) || (implementor.IsGenericType && (implementor.GetGenericTypeDefinition() == serviceType)));
        }

        public static bool TryBindServiceType(Service service, IEnumerable<Service> configuredOpenGenericServices, Type openGenericImplementationType, out Type constructedImplementationType, out IEnumerable<Service> constructedServices)
        {
            var swt = service as IServiceWithType;

            if ( swt != null && swt.ServiceType.IsGenericType )
            {
                var definitionService = (IServiceWithType)swt.ChangeType(swt.ServiceType.GetGenericTypeDefinition());
                var serviceGenericArguments = swt.ServiceType.GetGenericArguments();

                if (configuredOpenGenericServices.Cast<IServiceWithType>().Any(s => s.Equals(definitionService)))
                {
                    var implementorGenericArguments = TryMapImplementationGenericArguments(
                        openGenericImplementationType, swt.ServiceType, definitionService.ServiceType, serviceGenericArguments);

                    if (!implementorGenericArguments.Any(a => a == null) &&
                        openGenericImplementationType.IsCompatibleWithGenericParameterConstraints(implementorGenericArguments))
                    {
                        var constructedImplementationTypeTmp = openGenericImplementationType.MakeGenericType(implementorGenericArguments);

                        // This needs looking at
                        var implementedServices = (from IServiceWithType s in configuredOpenGenericServices
                                                   let genericService = s.ServiceType.MakeGenericType(serviceGenericArguments)
                                                   where genericService.IsAssignableFrom(constructedImplementationTypeTmp)
                                                   select s.ChangeType(genericService)).ToArray();

                        if (implementedServices.Length > 0)
                        {
                            constructedImplementationType = constructedImplementationTypeTmp;
                            constructedServices = implementedServices;
                            return true;
                        }
                    }
                }
            }

            constructedImplementationType = null;
            constructedServices = null;

            return false;

            //IServiceWithType type = service as IServiceWithType;
            //if ((type != null) && type.ServiceType.IsGenericType)
            //{
            //    var selector = null;
            //    IServiceWithType definitionService = (IServiceWithType) type.ChangeType(type.ServiceType.GetGenericTypeDefinition());
            //    Type[] serviceGenericArguments = type.ServiceType.GetGenericArguments();
            //    if (configuredOpenGenericServices.Cast<IServiceWithType>().Any<IServiceWithType>(s => s.Equals(definitionService)))
            //    {
            //        Type[] source = TryMapImplementationGenericArguments(openGenericImplementationType, type.ServiceType, definitionService.ServiceType, serviceGenericArguments);
            //        if (!source.Any<Type>(a => (a == null)) && openGenericImplementationType.IsCompatibleWithGenericParameterConstraints(source))
            //        {
            //            Type constructedImplementationTypeTmp = openGenericImplementationType.MakeGenericType(source);
            //            if (selector == null)
            //            {
            //                selector = s => new { 
            //                    s = s,
            //                    genericService = s.ServiceType.MakeGenericType(serviceGenericArguments)
            //                };
            //            }

            //            Service[] serviceArray = configuredOpenGenericServices.Cast<IServiceWithType>().Select(selector)
            //                .Where( s => s.ServiceType.IsAssignableFrom(constructedImplementationTypeTmp) )
            //                .Select( s => s.ChangeType(genericService)).ToArray<Service>();

            //            //Service[] serviceArray = (from <>h__TransparentIdentifier0 in configuredOpenGenericServices.Cast<IServiceWithType>().Select(selector)
            //            //    where <>h__TransparentIdentifier0.genericService.IsAssignableFrom(constructedImplementationTypeTmp)
            //            //    select <>h__TransparentIdentifier0.s.ChangeType(<>h__TransparentIdentifier0.genericService)).ToArray<Service>();

            //            if (serviceArray.Length > 0)
            //            {
            //                constructedImplementationType = constructedImplementationTypeTmp;
            //                constructedServices = serviceArray;
            //                return true;
            //            }
            //        }
            //    }
            //}
            //constructedImplementationType = null;
            //constructedServices = null;
            //return false;
        }

        private static Type TryFindServiceArgumentForImplementationArgumentDefinition(Type implementationGenericArgumentDefinition, IEnumerable<KeyValuePair<Type, Type>> serviceArgumentDefinitionToArgument)
        {
            var matchingRegularType = serviceArgumentDefinitionToArgument
               .Where(argdef => !argdef.Key.IsGenericType && implementationGenericArgumentDefinition.Name == argdef.Key.Name)
               .Select(argdef => argdef.Value)
               .FirstOrDefault();

            if (matchingRegularType != null)
                return matchingRegularType;

            return serviceArgumentDefinitionToArgument
                .Where(argdef => argdef.Key.IsGenericType && argdef.Value.GetGenericArguments().Length > 0)
                .Select(argdef => TryFindServiceArgumentForImplementationArgumentDefinition(
                    implementationGenericArgumentDefinition, argdef.Key.GetGenericArguments().Zip(argdef.Value.GetGenericArguments(), (a, b) => new KeyValuePair<Type, Type>(a, b))))
                .FirstOrDefault(x => x != null);

            //Type type = (from argdef in serviceArgumentDefinitionToArgument
            //    where !argdef.Key.IsGenericType && (implementationGenericArgumentDefinition.Name == argdef.Key.Name)
            //    select argdef.Value).FirstOrDefault<Type>();
            //if (type != null)
            //{
            //    return type;
            //}
            //return (from argdef in serviceArgumentDefinitionToArgument
            //    where argdef.Key.IsGenericType && (argdef.Value.GetGenericArguments().Length > 0)
            //    select TryFindServiceArgumentForImplementationArgumentDefinition(implementationGenericArgumentDefinition, argdef.Key.GetGenericArguments().Zip<Type, Type, KeyValuePair<Type, Type>>(argdef.Value.GetGenericArguments(), (a, b) => new KeyValuePair<Type, Type>(a, b)))).FirstOrDefault<Type>();
        }

        private static Type[] TryMapImplementationGenericArguments(Type implementationType, Type serviceType, Type serviceTypeDefinition, Type[] serviceGenericArguments)
        {
            if (serviceTypeDefinition == implementationType)
            {
                return serviceGenericArguments;
            }
            Type[] genericArguments = implementationType.GetGenericArguments();
            IEnumerable<KeyValuePair<Type, Type>> serviceArgumentDefinitionToArgumentMapping = (serviceType.IsInterface ? GetInterface(implementationType, serviceType).GetGenericArguments() : serviceTypeDefinition.GetGenericArguments()).Zip<Type, Type, KeyValuePair<Type, Type>>(serviceGenericArguments, (a, b) => new KeyValuePair<Type, Type>(a, b));
            return (from implementationGenericArgumentDefinition in genericArguments select TryFindServiceArgumentForImplementationArgumentDefinition(implementationGenericArgumentDefinition, serviceArgumentDefinitionToArgumentMapping)).ToArray<Type>();
        }
    }
}


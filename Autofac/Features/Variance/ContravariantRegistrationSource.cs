namespace Autofac.Features.Variance
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ContravariantRegistrationSource : IRegistrationSource
    {
        private const string IsContravariantAdapter = "IsContravariantAdapter";

        private static IEnumerable<Type> GetBagOfTypesAssignableFrom(Type type)
        {
            if (type.BaseType != null)
            {
                yield return type.BaseType;

                foreach (var fromBase in GetBagOfTypesAssignableFrom(type.BaseType))
                    yield return fromBase;
            }
            else
            {
                if (type != typeof(object))
                    yield return typeof(object);
            }

            foreach (var ifce in type.GetInterfaces())
            {
                if (ifce != type)
                {
                    yield return ifce;

                    foreach (var fromIfce in GetBagOfTypesAssignableFrom(ifce))
                        yield return fromIfce;
                }
            }

            //return new <GetBagOfTypesAssignableFrom>d__c(-2) { <>3__type = type };
        }

        private static IEnumerable<Type> GetTypesAssignableFrom(Type type)
        {
            return GetBagOfTypesAssignableFrom(type).Distinct<Type>();
        }

        private static bool IsCompatibleInterfaceType(Type type, out int contravariantParameterIndex)
        {
            if (type.IsGenericType && type.IsInterface)
            {
                var typeArray = (from cwi in type.GetGenericTypeDefinition().GetGenericArguments().Select((c, i) => new { 
                    IsContravariant = (c.GenericParameterAttributes & GenericParameterAttributes.Contravariant) != GenericParameterAttributes.None,
                    Index = i
                })
                    where cwi.IsContravariant
                    select cwi).ToArray();
                if (typeArray.Length == 1)
                {
                    contravariantParameterIndex = typeArray[0].Index;
                    return true;
                }
            }
            contravariantParameterIndex = 0;
            return false;
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (registrationAccessor == null) throw new ArgumentNullException("registrationAccessor");

            int contravariantParameterIndex;
            var swt = service as IServiceWithType;
            if (swt == null || !IsCompatibleInterfaceType(swt.ServiceType, out contravariantParameterIndex))
                return Enumerable.Empty<IComponentRegistration>();

            var args = swt.ServiceType.GetGenericArguments();
            var definition = swt.ServiceType.GetGenericTypeDefinition();
            var contravariantParameter = args[contravariantParameterIndex];
            var possibleSubstitutions = GetTypesAssignableFrom(contravariantParameter);
            var variations = possibleSubstitutions
                .Select(s => SubstituteArrayElementAt(args, s, contravariantParameterIndex))
                .Where(a => definition.IsCompatibleWithGenericParameterConstraints(a))
                .Select(a => definition.MakeGenericType(a));
            var variantRegistrations = variations
                .SelectMany(v => registrationAccessor(swt.ChangeType(v)))
                .Where(r => !r.Metadata.ContainsKey(IsContravariantAdapter));

            return variantRegistrations
                .Select(vr => RegistrationBuilder
                    .ForDelegate((c, p) => c.ResolveComponent(vr, p))
                    .Targeting(vr)
                    .As(service)
                    .WithMetadata(IsContravariantAdapter, true)
                    .CreateRegistration());

            //int contravariantParameterIndex;
            //if (service == null)
            //{
            //    throw new ArgumentNullException("service");
            //}
            //if (registrationAccessor == null)
            //{
            //    throw new ArgumentNullException("registrationAccessor");
            //}
            //IServiceWithType swt = service as IServiceWithType;
            //if ((swt == null) || !IsCompatibleInterfaceType(swt.ServiceType, out contravariantParameterIndex))
            //{
            //    return Enumerable.Empty<IComponentRegistration>();
            //}
            //Type[] args = swt.ServiceType.GetGenericArguments();
            //Type definition = swt.ServiceType.GetGenericTypeDefinition();
            //Type type = args[contravariantParameterIndex];
            //return (from v in from s in GetTypesAssignableFrom(type)
            //    select SubstituteArrayElementAt(args, s, contravariantParameterIndex) into a
            //    where definition.IsCompatibleWithGenericParameterConstraints(a)
            //    select definition.MakeGenericType(a)
            //    select registrationAccessor(swt.ChangeType(v)) into vr
            //    where !vr.Metadata.ContainsKey("IsContravariantAdapter")
            //    select RegistrationBuilder.ForDelegate<object>((c, p) => c.ResolveComponent(vr, p)).Targeting<object, SimpleActivatorData, SingleRegistrationStyle>(vr).As(new Service[] { service }).WithMetadata("IsContravariantAdapter", true).CreateRegistration<object, SimpleActivatorData, SingleRegistrationStyle>());
        }

        private static Type[] SubstituteArrayElementAt(Type[] array, Type newElement, int index)
        {
            Type[] typeArray = array.ToArray<Type>();
            typeArray[index] = newElement;
            return typeArray;
        }

        public bool IsAdapterForIndividualComponents
        {
            get
            {
                return true;
            }
        }
    }
}


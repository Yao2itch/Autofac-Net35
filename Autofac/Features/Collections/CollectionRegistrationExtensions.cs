namespace Autofac.Features.Collections
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class CollectionRegistrationExtensions
    {
        private const string MemberOfPropertyKey = "Autofac.CollectionRegistrationExtensions.MemberOf";

        private static IEnumerable<IComponentRegistration> GetElementRegistrations(string collectionName, IComponentRegistry registry)
        {
            return (from cr in registry.Registrations
                where IsElementRegistration(collectionName, cr)
                select cr);
        }

        private static bool IsElementRegistration(string collectionName, IComponentRegistration cr)
        {
            object obj2;
            return (cr.Metadata.TryGetValue("Autofac.CollectionRegistrationExtensions.MemberOf", out obj2) && ((IEnumerable<string>) obj2).Contains<string>(collectionName));
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> MemberOf<TLimit, TActivatorData, TSingleRegistrationStyle>(IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, string collectionName) where TSingleRegistrationStyle: SingleRegistrationStyle
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            Enforce.ArgumentNotNullOrEmpty(collectionName, "collectionName");
            registration.OnRegistered<TLimit, TActivatorData, TSingleRegistrationStyle>(delegate (ComponentRegisteredEventArgs e) {
                IDictionary<string, object> metadata = e.ComponentRegistration.Metadata;
                if (metadata.ContainsKey("Autofac.CollectionRegistrationExtensions.MemberOf"))
                {
                    metadata["Autofac.CollectionRegistrationExtensions.MemberOf"] = ((IEnumerable<string>) metadata["Autofac.CollectionRegistrationExtensions.MemberOf"]).Union<string>(new string[] { collectionName });
                }
                else
                {
                    metadata.Add("Autofac.CollectionRegistrationExtensions.MemberOf", new string[] { collectionName });
                }
            });
            return registration;
        }

        public static IRegistrationBuilder<T[], SimpleActivatorData, SingleRegistrationStyle> RegisterCollection<T>(ContainerBuilder builder, string collectionName, Type elementType)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            Enforce.ArgumentNotNullOrEmpty(collectionName, "collectionName");
            DelegateActivator activator = new DelegateActivator(elementType.MakeArrayType(), delegate (IComponentContext c, IEnumerable<Parameter> p) {
                object[] objArray = (from e in GetElementRegistrations(collectionName, c.ComponentRegistry) select c.ResolveComponent(e, p)).ToArray<object>();
                Array array = Array.CreateInstance(elementType, objArray.Length);
                objArray.CopyTo(array, 0);
                return array;
            });
            RegistrationBuilder<T[], SimpleActivatorData, SingleRegistrationStyle> rb = new RegistrationBuilder<T[], SimpleActivatorData, SingleRegistrationStyle>(new TypedService(typeof(T[])), new SimpleActivatorData(activator), new SingleRegistrationStyle());
            builder.RegisterCallback(delegate (IComponentRegistry cr) {
                RegistrationBuilder.RegisterSingleComponent<T[], SimpleActivatorData, SingleRegistrationStyle>(cr, rb);
            });
            return rb;
        }
    }
}


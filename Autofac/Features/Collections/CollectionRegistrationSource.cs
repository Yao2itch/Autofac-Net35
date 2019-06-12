namespace Autofac.Features.Collections
{
    using Autofac.Core;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Core.Lifetime;
    using Autofac.Core.Registration;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class CollectionRegistrationSource : IRegistrationSource
    {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (registrationAccessor == null)
            {
                throw new ArgumentNullException("registrationAccessor");
            }
            IServiceWithType type = service as IServiceWithType;
            if (type != null)
            {
                Type serviceType = type.ServiceType;
                Type elementType = null;
                if (serviceType.IsGenericTypeDefinedBy(typeof(IEnumerable<>)))
                {
                    elementType = serviceType.GetGenericArguments()[0];
                }
                else if (serviceType.IsArray)
                {
                    elementType = serviceType.GetElementType();
                }
                if (elementType != null)
                {
                    Service elementTypeService = type.ChangeType(elementType);
                    Type limitType = elementType.MakeArrayType();
                    ComponentRegistration registration = new ComponentRegistration(Guid.NewGuid(), new DelegateActivator(limitType, delegate (IComponentContext c, IEnumerable<Parameter> p) {
                        object[] objArray = (from cr in c.ComponentRegistry.RegistrationsFor(elementTypeService) select c.ResolveComponent(cr, p)).ToArray<object>();
                        Array array = Array.CreateInstance(elementType, objArray.Length);
                        objArray.CopyTo(array, 0);
                        return array;
                    }), new CurrentScopeLifetime(), InstanceSharing.None, InstanceOwnership.ExternallyOwned, new Service[] { service }, new Dictionary<string, object>());
                    return new IComponentRegistration[] { registration };
                }
            }
            return Enumerable.Empty<IComponentRegistration>();
        }

        public override string ToString()
        {
            return CollectionRegistrationSourceResources.CollectionRegistrationSourceDescription;
        }

        public bool IsAdapterForIndividualComponents
        {
            get
            {
                return false;
            }
        }
    }
}


namespace Autofac.Features.OwnedInstances
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class OwnedInstanceRegistrationSource : IRegistrationSource
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
            IServiceWithType ts = service as IServiceWithType;
            if ((ts == null) || !ts.ServiceType.IsGenericTypeDefinedBy(typeof(Owned<>)))
            {
                return Enumerable.Empty<IComponentRegistration>();
            }
            Type newType = ts.ServiceType.GetGenericArguments()[0];
            Service ownedInstanceService = ts.ChangeType(newType);
            return (from r in registrationAccessor(ownedInstanceService) select RegistrationBuilder.ForDelegate(ts.ServiceType, delegate (IComponentContext c, IEnumerable<Parameter> p) {
                object obj3;
                ILifetimeScope scope = c.Resolve<ILifetimeScope>().BeginLifetimeScope(ownedInstanceService);
                try
                {
                    object obj2 = scope.ResolveComponent(r, p);
                    obj3 = Activator.CreateInstance(ts.ServiceType, new object[] { obj2, scope });
                }
                catch
                {
                    scope.Dispose();
                    throw;
                }
                return obj3;
            }).ExternallyOwned().As(new Service[] { service }).Targeting<object, SimpleActivatorData, SingleRegistrationStyle>(r).CreateRegistration<object, SimpleActivatorData, SingleRegistrationStyle>());
        }

        public override string ToString()
        {
            return OwnedInstanceRegistrationSourceResources.OwnedInstanceRegistrationSourceDescription;
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


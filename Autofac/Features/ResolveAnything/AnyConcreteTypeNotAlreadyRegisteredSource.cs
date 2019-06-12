namespace Autofac.Features.ResolveAnything
{
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AnyConcreteTypeNotAlreadyRegisteredSource : IRegistrationSource
    {
        private readonly Func<Type, bool> _predicate;

        public AnyConcreteTypeNotAlreadyRegisteredSource() : this(t => true)
        {
        }

        public AnyConcreteTypeNotAlreadyRegisteredSource(Func<Type, bool> predicate)
        {
            this._predicate = Enforce.ArgumentNotNull<Func<Type, bool>>(predicate, "predicate");
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            TypedService service2 = service as TypedService;
            if ((((service2 == null) || !service2.ServiceType.IsClass) || (service2.ServiceType.IsSubclassOf(typeof(Delegate)) || service2.ServiceType.IsAbstract)) || (!this._predicate(service2.ServiceType) || registrationAccessor(service).Any<IComponentRegistration>()))
            {
                return Enumerable.Empty<IComponentRegistration>();
            }
            return new IComponentRegistration[] { RegistrationBuilder.ForType(service2.ServiceType).CreateRegistration<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>() };
        }

        public override string ToString()
        {
            return AnyConcreteTypeNotAlreadyRegisteredSourceResources.AnyConcreteTypeNotAlreadyRegisteredSourceDescription;
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


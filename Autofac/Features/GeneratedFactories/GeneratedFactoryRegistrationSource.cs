namespace Autofac.Features.GeneratedFactories
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class GeneratedFactoryRegistrationSource : IRegistrationSource
    {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Func<IComponentRegistration, IComponentRegistration> selector = null;
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (registrationAccessor == null)
            {
                throw new ArgumentNullException("registrationAccessor");
            }
            IServiceWithType ts = service as IServiceWithType;
            if ((ts == null) || !ts.ServiceType.IsDelegate())
            {
                return Enumerable.Empty<IComponentRegistration>();
            }
            Type newType = ts.ServiceType.FunctionReturnType();
            Service arg = ts.ChangeType(newType);
            if (selector == null)
            {
                selector = r => RegistrationBuilder.ForDelegate(ts.ServiceType, new Func<IComponentContext, IEnumerable<Parameter>, object>(new FactoryGenerator(ts.ServiceType, r, ParameterMapping.Adaptive).GenerateFactory)).InstancePerLifetimeScope().ExternallyOwned().As(new Service[] { service }).Targeting<object, SimpleActivatorData, SingleRegistrationStyle>(r).CreateRegistration<object, SimpleActivatorData, SingleRegistrationStyle>();
            }
            return registrationAccessor(arg).Select<IComponentRegistration, IComponentRegistration>(selector);
        }

        public override string ToString()
        {
            return GeneratedFactoryRegistrationSourceResources.GeneratedFactoryRegistrationSourceDescription;
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


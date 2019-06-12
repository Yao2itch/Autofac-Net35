namespace Autofac.Core.Registration
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class ExternalRegistrySource : IRegistrationSource
    {
        private readonly IComponentRegistry _registry;

        public ExternalRegistrySource(IComponentRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }
            this._registry = registry;
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            // Issue #475: This method was refactored significantly to handle
            // registrations made on the fly in parent lifetime scopes to correctly
            // pass to child lifetime scopes.
            foreach (var registration in _registry.RegistrationsFor(service).Where(r => !r.IsAdapting()))
            {
                var r = registration;
                yield return RegistrationBuilder.ForDelegate(r.Activator.LimitType, (c, p) => c.ResolveComponent(r, p))
                    .Targeting(r)
                    .As(service)
                    .ExternallyOwned()
                    .CreateRegistration();
            }

            //return new <RegistrationsFor>d__a(-2) { 
            //    <>4__this = this,
            //    <>3__service = service,
            //    <>3__registrationAccessor = registrationAccessor
            //};
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


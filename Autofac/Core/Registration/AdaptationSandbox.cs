namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class AdaptationSandbox
    {
        private readonly IEnumerable<IRegistrationSource> _adapters;
        private readonly IComponentRegistration _registration;
        private readonly IEnumerable<Service> _adapterServices;
        private readonly IDictionary<Service, IList<IRegistrationSource>> _adaptersToQuery = new Dictionary<Service, IList<IRegistrationSource>>();
        private readonly IList<IComponentRegistration> _registrations = new List<IComponentRegistration>();

        public AdaptationSandbox(IEnumerable<IRegistrationSource> adapters, IComponentRegistration registration, IEnumerable<Service> adapterServices)
        {
            this._adapters = adapters;
            this._registration = registration;
            this._adapterServices = adapterServices;
            this._registrations.Add(this._registration);
        }

        public IEnumerable<IComponentRegistration> GetAdapters()
        {
            foreach (Service service in this._adapterServices)
            {
                this.GetAndInitialiseRegistrationsFor(service);
            }
            return (from r in this._registrations
                where r != this._registration
                select r);
        }

        private IEnumerable<IComponentRegistration> GetAndInitialiseRegistrationsFor(Service service)
        {
            IList<IRegistrationSource> list;
            if (!this._adaptersToQuery.TryGetValue(service, out list))
            {
                list = new List<IRegistrationSource>(this._adapters);
                this._adaptersToQuery.Add(service, list);
            }
            foreach (IRegistrationSource source in this._adapters)
            {
                list.Remove(source);
                IComponentRegistration[] items = source.RegistrationsFor(service, new Func<Service, IEnumerable<IComponentRegistration>>(this.GetAndInitialiseRegistrationsFor)).ToArray<IComponentRegistration>();
                this._registrations.AddRange<IComponentRegistration>(items);
            }
            return (from r in this._registrations
                where r.Services.Contains<Service>(service)
                select r);
        }
    }
}


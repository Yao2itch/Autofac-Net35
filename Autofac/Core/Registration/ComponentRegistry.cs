namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ComponentRegistry : Disposable, IComponentRegistry, IDisposable
    {
        private readonly object _synchRoot = new object();
        private readonly IList<IRegistrationSource> _dynamicRegistrationSources = new List<IRegistrationSource>();
        private readonly ICollection<IComponentRegistration> _registrations = new List<IComponentRegistration>();
        private readonly IDictionary<Service, ServiceRegistrationInfo> _serviceInfo = new Dictionary<Service, ServiceRegistrationInfo>();

        public event EventHandler<ComponentRegisteredEventArgs> Registered;

        public event EventHandler<RegistrationSourceAddedEventArgs> RegistrationSourceAdded;

        private void AddRegistration(IComponentRegistration registration, bool preserveDefaults)
        {
            foreach (Service service in registration.Services)
            {
                this.GetServiceInfo(service).AddImplementation(registration, preserveDefaults);
            }
            this._registrations.Add(registration);
            EventHandler<ComponentRegisteredEventArgs> registered = this.Registered;
            if (registered != null)
            {
                registered(this, new ComponentRegisteredEventArgs(this, registration));
            }
        }

        public void AddRegistrationSource(IRegistrationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            lock (this._synchRoot)
            {
                this._dynamicRegistrationSources.Insert(0, source);
                foreach (KeyValuePair<Service, ServiceRegistrationInfo> pair in this._serviceInfo)
                {
                    pair.Value.Include(source);
                }
                EventHandler<RegistrationSourceAddedEventArgs> registrationSourceAdded = this.RegistrationSourceAdded;
                if (registrationSourceAdded != null)
                {
                    registrationSourceAdded(this, new RegistrationSourceAddedEventArgs(this, source));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach (IComponentRegistration registration in this._registrations)
            {
                registration.Dispose();
            }
            base.Dispose(disposing);
        }

        private ServiceRegistrationInfo GetInitializedServiceInfo(Service service)
        {
            ServiceRegistrationInfo serviceInfo = this.GetServiceInfo(service);
            if (!serviceInfo.IsInitialized)
            {
                if (!serviceInfo.IsInitializing)
                {
                    serviceInfo.BeginInitialization(this._dynamicRegistrationSources);
                }
                while (serviceInfo.HasSourcesToQuery)
                {
                    Func<IRegistrationSource, bool> predicate = null;
                    IRegistrationSource next = serviceInfo.DequeueNextSource();
                    foreach (IComponentRegistration registration in next.RegistrationsFor(service, new Func<Service, IEnumerable<IComponentRegistration>>(this.RegistrationsFor)))
                    {
                        foreach (Service service2 in registration.Services)
                        {
                            ServiceRegistrationInfo info2 = this.GetServiceInfo(service2);
                            if (!info2.IsInitialized)
                            {
                                if (!info2.IsInitializing)
                                {
                                    if (predicate == null)
                                    {
                                        predicate = src => src != next;
                                    }
                                    info2.BeginInitialization(this._dynamicRegistrationSources.Where<IRegistrationSource>(predicate));
                                }
                                else
                                {
                                    info2.SkipSource(next);
                                }
                            }
                        }
                        this.AddRegistration(registration, true);
                    }
                }
                serviceInfo.CompleteInitialization();
            }
            return serviceInfo;
        }

        private ServiceRegistrationInfo GetServiceInfo(Service service)
        {
            ServiceRegistrationInfo info;
            if (this._serviceInfo.TryGetValue(service, out info))
            {
                return info;
            }
            ServiceRegistrationInfo info2 = new ServiceRegistrationInfo(service);
            this._serviceInfo.Add(service, info2);
            return info2;
        }

        public bool IsRegistered(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            lock (this._synchRoot)
            {
                return this.GetInitializedServiceInfo(service).IsRegistered;
            }
        }

        public void Register(IComponentRegistration registration)
        {
            this.Register(registration, false);
        }

        public virtual void Register(IComponentRegistration registration, bool preserveDefaults)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            lock (this._synchRoot)
            {
                this.AddRegistration(registration, preserveDefaults);
                this.UpdateInitialisedAdapters(registration);
            }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            lock (this._synchRoot)
            {
                return this.GetInitializedServiceInfo(service).Implementations.ToArray<IComponentRegistration>();
            }
        }

        public bool TryGetRegistration(Service service, out IComponentRegistration registration)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            lock (this._synchRoot)
            {
                return this.GetInitializedServiceInfo(service).TryGetRegistration(out registration);
            }
        }

        private void UpdateInitialisedAdapters(IComponentRegistration registration)
        {
            Service[] adapterServices = (from si in this._serviceInfo
                where si.Value.ShouldRecalculateAdaptersOn(registration)
                select si.Key).ToArray<Service>();
            if (adapterServices.Length != 0)
            {
                AdaptationSandbox sandbox = new AdaptationSandbox(from rs in this._dynamicRegistrationSources
                    where rs.IsAdapterForIndividualComponents
                    select rs, registration, adapterServices);
                foreach (IComponentRegistration registration2 in sandbox.GetAdapters())
                {
                    this.AddRegistration(registration2, true);
                }
            }
        }

        public IEnumerable<IComponentRegistration> Registrations
        {
            get
            {
                lock (this._synchRoot)
                {
                    return this._registrations.ToArray<IComponentRegistration>();
                }
            }
        }

        public IEnumerable<IRegistrationSource> Sources
        {
            get
            {
                lock (this._synchRoot)
                {
                    return this._dynamicRegistrationSources.ToArray<IRegistrationSource>();
                }
            }
        }

        public bool HasLocalComponents
        {
            get
            {
                return true;
            }
        }
    }
}


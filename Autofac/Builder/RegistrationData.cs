namespace Autofac.Builder
{
    using Autofac.Core;
    using Core.Lifetime;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RegistrationData
    {
        private bool _defaultServiceOverridden;
        private Service _defaultService;
        private readonly ICollection<Service> _services = new List<Service>();
        private InstanceOwnership _ownership = InstanceOwnership.OwnedByLifetimeScope;
        private IComponentLifetime _lifetime = new CurrentScopeLifetime();
        private InstanceSharing _sharing;
        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>();
        private readonly ICollection<EventHandler<PreparingEventArgs>> _preparingHandlers = new List<EventHandler<PreparingEventArgs>>();
        private readonly ICollection<EventHandler<ActivatingEventArgs<object>>> _activatingHandlers = new List<EventHandler<ActivatingEventArgs<object>>>();
        private readonly ICollection<EventHandler<ActivatedEventArgs<object>>> _activatedHandlers = new List<EventHandler<ActivatedEventArgs<object>>>();

        public RegistrationData(Service defaultService)
        {
            if (defaultService == null)
            {
                throw new ArgumentNullException("defaultService");
            }
            this._defaultService = defaultService;
        }

        private static void AddAll<T>(ICollection<T> to, IEnumerable<T> from)
        {
            foreach (T local in from)
            {
                to.Add(local);
            }
        }

        public void AddService(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            this._defaultServiceOverridden = true;
            this._services.Add(service);
        }

        public void AddServices(IEnumerable<Service> services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            this._defaultServiceOverridden = true;
            foreach (Service service in services)
            {
                this.AddService(service);
            }
        }

        public void ClearServices()
        {
            this._services.Clear();
            this._defaultServiceOverridden = true;
        }

        public void CopyFrom(RegistrationData that, bool includeDefaultService)
        {
            this.Ownership = that.Ownership;
            this.Sharing = that.Sharing;
            this.Lifetime = that.Lifetime;
            this._defaultServiceOverridden |= that._defaultServiceOverridden;
            if (includeDefaultService)
            {
                this._defaultService = that._defaultService;
            }
            AddAll<Service>(this._services, that._services);
            AddAll<KeyValuePair<string, object>>(this.Metadata, that.Metadata);
            AddAll<EventHandler<PreparingEventArgs>>(this.PreparingHandlers, that.PreparingHandlers);
            AddAll<EventHandler<ActivatingEventArgs<object>>>(this.ActivatingHandlers, that.ActivatingHandlers);
            AddAll<EventHandler<ActivatedEventArgs<object>>>(this.ActivatedHandlers, that.ActivatedHandlers);
        }

        public IEnumerable<Service> Services
        {
            get
            {
                if (this._defaultServiceOverridden)
                {
                    return this._services;
                }
                return this._services.DefaultIfEmpty<Service>(this._defaultService);
            }
        }

        public InstanceOwnership Ownership
        {
            get
            {
                return this._ownership;
            }
            set
            {
                this._ownership = value;
            }
        }

        public IComponentLifetime Lifetime
        {
            get
            {
                return this._lifetime;
            }
            set
            {
                this._lifetime = Enforce.ArgumentNotNull<IComponentLifetime>(value, "lifetime");
            }
        }

        public InstanceSharing Sharing
        {
            get
            {
                return this._sharing;
            }
            set
            {
                this._sharing = value;
            }
        }

        public IDictionary<string, object> Metadata
        {
            get
            {
                return this._metadata;
            }
        }

        public ICollection<EventHandler<PreparingEventArgs>> PreparingHandlers
        {
            get
            {
                return this._preparingHandlers;
            }
        }

        public ICollection<EventHandler<ActivatingEventArgs<object>>> ActivatingHandlers
        {
            get
            {
                return this._activatingHandlers;
            }
        }

        public ICollection<EventHandler<ActivatedEventArgs<object>>> ActivatedHandlers
        {
            get
            {
                return this._activatedHandlers;
            }
        }
    }
}


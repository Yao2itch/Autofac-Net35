namespace Autofac.Core.Registration
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ComponentRegistration : Disposable, IComponentRegistration, IDisposable
    {
        private readonly IComponentRegistration _target;

        public event EventHandler<ActivatedEventArgs<object>> Activated;

        public event EventHandler<ActivatingEventArgs<object>> Activating;

        public event EventHandler<PreparingEventArgs> Preparing;

        public ComponentRegistration(Guid id, IInstanceActivator activator, IComponentLifetime lifetime, InstanceSharing sharing, InstanceOwnership ownership, IEnumerable<Service> services, IDictionary<string, object> metadata)
        {
            this.Id = id;
            this.Activator = Enforce.ArgumentNotNull<IInstanceActivator>(activator, "activator");
            this.Lifetime = Enforce.ArgumentNotNull<IComponentLifetime>(lifetime, "lifetime");
            this.Sharing = sharing;
            this.Ownership = ownership;
            this.Services = Enforce.ArgumentElementNotNull<IEnumerable<Service>>(Enforce.ArgumentNotNull<IEnumerable<Service>>(services, "services"), "services").ToList<Service>();
            this.Metadata = new Dictionary<string, object>(Enforce.ArgumentNotNull<IDictionary<string, object>>(metadata, "metadata"));
        }

        public ComponentRegistration(Guid id, IInstanceActivator activator, IComponentLifetime lifetime, InstanceSharing sharing, InstanceOwnership ownership, IEnumerable<Service> services, IDictionary<string, object> metadata, IComponentRegistration target) : this(id, activator, lifetime, sharing, ownership, services, metadata)
        {
            this._target = Enforce.ArgumentNotNull<IComponentRegistration>(target, "target");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Activator.Dispose();
            }
        }

        public void RaiseActivated(IComponentContext context, IEnumerable<Parameter> parameters, object instance)
        {
            EventHandler<ActivatedEventArgs<object>> activated = this.Activated;
            if (activated != null)
            {
                ActivatedEventArgs<object> e = new ActivatedEventArgs<object>(context, this, parameters, instance);
                activated(this, e);
            }
        }

        public void RaiseActivating(IComponentContext context, IEnumerable<Parameter> parameters, ref object instance)
        {
            EventHandler<ActivatingEventArgs<object>> activating = this.Activating;
            if (activating != null)
            {
                ActivatingEventArgs<object> e = new ActivatingEventArgs<object>(context, this, parameters, instance);
                activating(this, e);
                instance = e.Instance;
            }
        }

        public void RaisePreparing(IComponentContext context, ref IEnumerable<Parameter> parameters)
        {
            EventHandler<PreparingEventArgs> preparing = this.Preparing;
            if (preparing != null)
            {
                PreparingEventArgs e = new PreparingEventArgs(context, this, parameters);
                preparing(this, e);
                parameters = e.Parameters;
            }
        }

        public override string ToString()
        {
            object[] args = new object[] { this.Activator, (from s in this.Services select s.Description).JoinWith(", "), this.Lifetime, this.Sharing, this.Ownership };
            return string.Format(CultureInfo.CurrentCulture, ComponentRegistrationResources.ToStringFormat, args);
        }

        public IComponentRegistration Target
        {
            get
            {
                return (this._target ?? this);
            }
        }

        public Guid Id { get; private set; }

        public IInstanceActivator Activator { get; set; }

        public IComponentLifetime Lifetime { get; private set; }

        public InstanceSharing Sharing { get; private set; }

        public InstanceOwnership Ownership { get; private set; }

        public IEnumerable<Service> Services { get; private set; }

        public IDictionary<string, object> Metadata { get; private set; }
    }
}


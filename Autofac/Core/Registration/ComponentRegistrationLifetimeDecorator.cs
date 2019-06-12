namespace Autofac.Core.Registration
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    internal class ComponentRegistrationLifetimeDecorator : Disposable, IComponentRegistration, IDisposable
    {
        private readonly IComponentLifetime _lifetime;
        private readonly IComponentRegistration _inner;

        public event EventHandler<ActivatedEventArgs<object>> Activated
        {
            add
            {
                this._inner.Activated += value;
            }
            remove
            {
                this._inner.Activated -= value;
            }
        }

        public event EventHandler<ActivatingEventArgs<object>> Activating
        {
            add
            {
                this._inner.Activating += value;
            }
            remove
            {
                this._inner.Activating -= value;
            }
        }

        public event EventHandler<PreparingEventArgs> Preparing
        {
            add
            {
                this._inner.Preparing += value;
            }
            remove
            {
                this._inner.Preparing -= value;
            }
        }

        public ComponentRegistrationLifetimeDecorator(IComponentRegistration inner, IComponentLifetime lifetime)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (lifetime == null)
            {
                throw new ArgumentNullException("lifetime");
            }
            this._inner = inner;
            this._lifetime = lifetime;
        }

        public void RaiseActivated(IComponentContext context, IEnumerable<Parameter> parameters, object instance)
        {
            this._inner.RaiseActivated(context, parameters, instance);
        }

        public void RaiseActivating(IComponentContext context, IEnumerable<Parameter> parameters, ref object instance)
        {
            this._inner.RaiseActivating(context, parameters, ref instance);
        }

        public void RaisePreparing(IComponentContext context, ref IEnumerable<Parameter> parameters)
        {
            this._inner.RaisePreparing(context, ref parameters);
        }

        public Guid Id
        {
            get
            {
                return this._inner.Id;
            }
        }

        public IInstanceActivator Activator
        {
            get
            {
                return this._inner.Activator;
            }
        }

        public IComponentLifetime Lifetime
        {
            get
            {
                return this._lifetime;
            }
        }

        public InstanceSharing Sharing
        {
            get
            {
                return this._inner.Sharing;
            }
        }

        public InstanceOwnership Ownership
        {
            get
            {
                return this._inner.Ownership;
            }
        }

        public IEnumerable<Service> Services
        {
            get
            {
                return this._inner.Services;
            }
        }

        public IDictionary<string, object> Metadata
        {
            get
            {
                return this._inner.Metadata;
            }
        }

        public IComponentRegistration Target
        {
            get
            {
                if (this._inner.IsAdapting())
                {
                    return this._inner.Target;
                }
                return this;
            }
        }
    }
}


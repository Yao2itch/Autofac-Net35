namespace Autofac.Core.Activators.ProvidedInstance
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators;
    using Util;
    using System;
    using System.Collections.Generic;

    public class ProvidedInstanceActivator : InstanceActivator, IInstanceActivator, IDisposable
    {
        private readonly object _instance;
        private bool _activated;
        private bool _disposeInstance;

        public ProvidedInstanceActivator(object instance) : base(Enforce.ArgumentNotNull<object>(instance, "instance").GetType())
        {
            this._instance = instance;
        }

        public object ActivateInstance(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (this._activated)
            {
                throw new InvalidOperationException(ProvidedInstanceActivatorResources.InstanceAlreadyActivated);
            }
            this._activated = true;
            return this._instance;
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing && this._disposeInstance) && (this._instance is IDisposable))
            {
                ((IDisposable) this._instance).Dispose();
            }
            base.Dispose(disposing);
        }

        public bool DisposeInstance
        {
            get
            {
                return this._disposeInstance;
            }
            set
            {
                this._disposeInstance = value;
            }
        }
    }
}


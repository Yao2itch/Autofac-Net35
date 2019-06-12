namespace Autofac.Core.Resolving
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class InstanceLookup : IComponentContext, IInstanceLookup
    {
        private readonly IEnumerable<Parameter> _parameters;
        private readonly IComponentRegistration _componentRegistration;
        private readonly IResolveOperation _context;
        private readonly ISharingLifetimeScope _activationScope;
        private object _newInstance;
        private bool _executed;

        public event EventHandler<InstanceLookupCompletionBeginningEventArgs> CompletionBeginning;

        public event EventHandler<InstanceLookupCompletionEndingEventArgs> CompletionEnding;

        public event EventHandler<InstanceLookupEndingEventArgs> InstanceLookupEnding;

        public InstanceLookup(IComponentRegistration registration, IResolveOperation context, ISharingLifetimeScope mostNestedVisibleScope, IEnumerable<Parameter> parameters)
        {
            this._parameters = parameters;
            this._componentRegistration = registration;
            this._context = context;
            this._activationScope = this._componentRegistration.Lifetime.FindScope(mostNestedVisibleScope);
        }

        private object Activate(IEnumerable<Parameter> parameters)
        {
            this._componentRegistration.RaisePreparing(this, ref parameters);
            this._newInstance = this._componentRegistration.Activator.ActivateInstance(this, parameters);
            if (this._componentRegistration.Ownership == InstanceOwnership.OwnedByLifetimeScope)
            {
                IDisposable instance = this._newInstance as IDisposable;
                if (instance != null)
                {
                    this._activationScope.Disposer.AddInstanceForDisposal(instance);
                }
            }
            this._componentRegistration.RaiseActivating(this, parameters, ref this._newInstance);
            return this._newInstance;
        }

        public void Complete()
        {
            if (this.NewInstanceActivated)
            {
                EventHandler<InstanceLookupCompletionBeginningEventArgs> completionBeginning = this.CompletionBeginning;
                if (completionBeginning != null)
                {
                    completionBeginning(this, new InstanceLookupCompletionBeginningEventArgs(this));
                }
                this._componentRegistration.RaiseActivated(this, this.Parameters, this._newInstance);
                EventHandler<InstanceLookupCompletionEndingEventArgs> completionEnding = this.CompletionEnding;
                if (completionEnding != null)
                {
                    completionEnding(this, new InstanceLookupCompletionEndingEventArgs(this));
                }
            }
        }

        public object Execute()
        {
            object orCreateAndShare;
            if (this._executed)
            {
                throw new InvalidOperationException(ComponentActivationResources.ActivationAlreadyExecuted);
            }
            this._executed = true;
            if (this._componentRegistration.Sharing == InstanceSharing.None)
            {
                orCreateAndShare = this.Activate(this.Parameters);
            }
            else
            {
                orCreateAndShare = this._activationScope.GetOrCreateAndShare(this._componentRegistration.Id, () => this.Activate(this.Parameters));
            }
            EventHandler<InstanceLookupEndingEventArgs> instanceLookupEnding = this.InstanceLookupEnding;
            if (instanceLookupEnding != null)
            {
                instanceLookupEnding(this, new InstanceLookupEndingEventArgs(this, this.NewInstanceActivated));
            }
            return orCreateAndShare;
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return this._context.GetOrCreateInstance(this._activationScope, registration, parameters);
        }

        private bool NewInstanceActivated
        {
            get
            {
                return (this._newInstance != null);
            }
        }

        public IComponentRegistry ComponentRegistry
        {
            get
            {
                return this._activationScope.ComponentRegistry;
            }
        }

        public IComponentRegistration ComponentRegistration
        {
            get
            {
                return this._componentRegistration;
            }
        }

        public ILifetimeScope ActivationScope
        {
            get
            {
                return this._activationScope;
            }
        }

        public IEnumerable<Parameter> Parameters
        {
            get
            {
                return this._parameters;
            }
        }
    }
}


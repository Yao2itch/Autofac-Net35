namespace Autofac.Core
{
    using Autofac;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Core.Lifetime;
    using Autofac.Core.Registration;
    using Resolving;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class Container : Disposable, IContainer, ILifetimeScope, IComponentContext, IDisposable, IServiceProvider
    {
        private readonly IComponentRegistry _componentRegistry = new Autofac.Core.Registration.ComponentRegistry();
        private readonly ILifetimeScope _rootLifetimeScope;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add
            {
                _rootLifetimeScope.ChildLifetimeScopeBeginning += value;
            }
            remove
            {
                _rootLifetimeScope.ChildLifetimeScopeBeginning -= value;
            }
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add
            {
                this._rootLifetimeScope.CurrentScopeEnding += value;
            }
            remove
            {
                this._rootLifetimeScope.CurrentScopeEnding -= value;
            }
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add
            {
                this._rootLifetimeScope.ResolveOperationBeginning += value;
            }
            remove
            {
                this._rootLifetimeScope.ResolveOperationBeginning -= value;
            }
        }

        internal Container()
        {
            this._componentRegistry.Register(new ComponentRegistration(LifetimeScope.SelfRegistrationId, new DelegateActivator(typeof(LifetimeScope), delegate (IComponentContext c, IEnumerable<Parameter> p) {
                throw new InvalidOperationException(ContainerResources.SelfRegistrationCannotBeActivated);
            }), new CurrentScopeLifetime(), InstanceSharing.Shared, InstanceOwnership.ExternallyOwned, new Service[] { new TypedService(typeof(ILifetimeScope)), new TypedService(typeof(IComponentContext)) }, new Dictionary<string, object>()));
            this._rootLifetimeScope = new LifetimeScope(this._componentRegistry);
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return this._rootLifetimeScope.BeginLifetimeScope();
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return this._rootLifetimeScope.BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return this._rootLifetimeScope.BeginLifetimeScope(tag);
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return this._rootLifetimeScope.BeginLifetimeScope(tag, configurationAction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._rootLifetimeScope.Dispose();
                this._componentRegistry.Dispose();
            }
            base.Dispose(disposing);
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return this._rootLifetimeScope.ResolveComponent(registration, parameters);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return ((IServiceProvider) this._rootLifetimeScope).GetService(serviceType);
        }

        public static Container Empty
        {
            get
            {
                return new Container();
            }
        }

        public IDisposer Disposer
        {
            get
            {
                return this._rootLifetimeScope.Disposer;
            }
        }

        public object Tag
        {
            get
            {
                return this._rootLifetimeScope.Tag;
            }
        }

        public IComponentRegistry ComponentRegistry
        {
            get
            {
                return this._componentRegistry;
            }
        }
    }
}


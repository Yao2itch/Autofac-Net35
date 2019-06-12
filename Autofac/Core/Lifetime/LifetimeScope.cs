namespace Autofac.Core.Lifetime
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Registration;
    using Autofac.Core.Resolving;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class LifetimeScope : Disposable, ISharingLifetimeScope, ILifetimeScope, IComponentContext, IDisposable, IServiceProvider
    {
        private readonly object _synchRoot;
        private readonly IDictionary<Guid, object> _sharedInstances;
        private readonly IComponentRegistry _componentRegistry;
        private readonly ISharingLifetimeScope _root;
        private readonly ISharingLifetimeScope _parent;
        private readonly IDisposer _disposer;
        private readonly object _tag;
        internal static Guid SelfRegistrationId = Guid.NewGuid();
        private static readonly Action<ContainerBuilder> NoConfiguration = delegate (ContainerBuilder b) {
        };
        public static readonly object RootTag = "root";

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning;

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding;

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning;

        private LifetimeScope()
        {
            this._synchRoot = new object();
            this._sharedInstances = new Dictionary<Guid, object>();
            this._disposer = new Autofac.Core.Disposer();
            this._sharedInstances[SelfRegistrationId] = this;
        }

        public LifetimeScope(IComponentRegistry componentRegistry) : this(componentRegistry, RootTag)
        {
        }

        public LifetimeScope(IComponentRegistry componentRegistry, object tag) : this()
        {
            this._componentRegistry = Enforce.ArgumentNotNull<IComponentRegistry>(componentRegistry, "componentRegistry");
            this._root = this;
            this._tag = Enforce.ArgumentNotNull<object>(tag, "tag");
        }

        protected LifetimeScope(IComponentRegistry componentRegistry, LifetimeScope parent, object tag) : this(componentRegistry, tag)
        {
            this._parent = Enforce.ArgumentNotNull<LifetimeScope>(parent, "parent");
            this._root = this._parent.RootLifetimeScope;
        }
        
        public ILifetimeScope BeginLifetimeScope()
        {
            return this.BeginLifetimeScope(MakeAnonymousTag());
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return this.BeginLifetimeScope(MakeAnonymousTag(), configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            this.CheckNotDisposed();
            CopyOnWriteRegistry componentRegistry = new CopyOnWriteRegistry(this._componentRegistry, () => this.CreateScopeRestrictedRegistry(tag, NoConfiguration));
            LifetimeScope scope = new LifetimeScope(componentRegistry, this, tag);
            this.RaiseBeginning(scope);
            return scope;
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            if (configurationAction == null)
            {
                throw new ArgumentNullException("configurationAction");
            }
            this.CheckNotDisposed();
            LifetimeScope scope = new LifetimeScope(this.CreateScopeRestrictedRegistry(tag, configurationAction), this, tag);
            this.RaiseBeginning(scope);
            return scope;
        }

        private void CheckNotDisposed()
        {
            if (base.IsDisposed)
            {
                Exception innerException = null;
                throw new ObjectDisposedException(LifetimeScopeResources.ScopeIsDisposed, innerException);
            }
        }

        private ScopeRestrictedRegistry CreateScopeRestrictedRegistry(object tag, Action<ContainerBuilder> configurationAction)
        {
            ContainerBuilder builder = new ContainerBuilder();
            foreach (IRegistrationSource source in from src in this.ComponentRegistry.Sources
                where src.IsAdapterForIndividualComponents
                select src)
            {
                builder.RegisterSource(source);
            }
            foreach (ExternalRegistrySource source2 in (from s in Traverse.Across<ISharingLifetimeScope>(this, s => s.ParentLifetimeScope)
                where s.ComponentRegistry.HasLocalComponents
                select new ExternalRegistrySource(s.ComponentRegistry)).Reverse<ExternalRegistrySource>())
            {
                builder.RegisterSource(source2);
            }
            configurationAction(builder);
            ScopeRestrictedRegistry componentRegistry = new ScopeRestrictedRegistry(tag);
            builder.Update(componentRegistry);
            return componentRegistry;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventHandler<LifetimeScopeEndingEventArgs> currentScopeEnding = this.CurrentScopeEnding;
                if (currentScopeEnding != null)
                {
                    currentScopeEnding(this, new LifetimeScopeEndingEventArgs(this));
                }
                this._disposer.Dispose();
            }
            base.Dispose(disposing);
        }

        public object GetOrCreateAndShare(Guid id, Func<object> creator)
        {
            lock (this._synchRoot)
            {
                object obj2;
                if (!this._sharedInstances.TryGetValue(id, out obj2))
                {
                    obj2 = creator();
                    this._sharedInstances.Add(id, obj2);
                }
                return obj2;
            }
        }

        private static object MakeAnonymousTag()
        {
            return new object();
        }

        private void RaiseBeginning(ILifetimeScope scope)
        {
            EventHandler<LifetimeScopeBeginningEventArgs> childLifetimeScopeBeginning = this.ChildLifetimeScopeBeginning;
            if (childLifetimeScopeBeginning != null)
            {
                childLifetimeScopeBeginning(this, new LifetimeScopeBeginningEventArgs(scope));
            }
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.CheckNotDisposed();
            lock (this._synchRoot)
            {
                ResolveOperation resolveOperation = new ResolveOperation(this);
                EventHandler<ResolveOperationBeginningEventArgs> resolveOperationBeginning = this.ResolveOperationBeginning;
                if (resolveOperationBeginning != null)
                {
                    resolveOperationBeginning(this, new ResolveOperationBeginningEventArgs(resolveOperation));
                }
                return resolveOperation.Execute(registration, parameters);
            }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.ResolveOptional(serviceType);
        }

        public ISharingLifetimeScope ParentLifetimeScope
        {
            get
            {
                return this._parent;
            }
        }

        public ISharingLifetimeScope RootLifetimeScope
        {
            get
            {
                return this._root;
            }
        }

        public IDisposer Disposer
        {
            get
            {
                return this._disposer;
            }
        }

        public object Tag
        {
            get
            {
                return this._tag;
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


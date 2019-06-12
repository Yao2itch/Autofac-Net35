namespace Autofac
{
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Features.Collections;
    using Autofac.Features.GeneratedFactories;
    using Autofac.Features.Indexed;
    using Autofac.Features.Metadata;
    using Autofac.Features.OwnedInstances;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class ContainerBuilder
    {
        private readonly IList<Action<IComponentRegistry>> _configurationCallbacks = new List<Action<IComponentRegistry>>();
        private bool _wasBuilt;

        public IContainer Build(ContainerBuildOptions options)
        {
            Container componentContext = new Container();
            this.Build(componentContext.ComponentRegistry, (options & ContainerBuildOptions.ExcludeDefaultModules) != ContainerBuildOptions.Default);
            if ((options & ContainerBuildOptions.IgnoreStartableComponents) == ContainerBuildOptions.Default)
            {
                StartStartableComponents(componentContext);
            }
            return componentContext;
        }

        private void Build(IComponentRegistry componentRegistry, bool excludeDefaultModules)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            if (this._wasBuilt)
            {
                throw new InvalidOperationException(ContainerBuilderResources.BuildCanOnlyBeCalledOnce);
            }
            this._wasBuilt = true;
            if (!excludeDefaultModules)
            {
                this.RegisterDefaultAdapters(componentRegistry);
            }
            foreach (Action<IComponentRegistry> action in this._configurationCallbacks)
            {
                action(componentRegistry);
            }
        }

        public virtual void RegisterCallback(Action<IComponentRegistry> configurationCallback)
        {
            this._configurationCallbacks.Add(Enforce.ArgumentNotNull<Action<IComponentRegistry>>(configurationCallback, "configurationCallback"));
        }

        private void RegisterDefaultAdapters(IComponentRegistry componentRegistry)
        {
            this.RegisterGeneric(typeof(KeyedServiceIndex<,>)).As(new Type[] { typeof(IIndex<,>) }).InstancePerLifetimeScope();
            componentRegistry.AddRegistrationSource(new CollectionRegistrationSource());
            componentRegistry.AddRegistrationSource(new OwnedInstanceRegistrationSource());
            componentRegistry.AddRegistrationSource(new MetaRegistrationSource());
            componentRegistry.AddRegistrationSource(new GeneratedFactoryRegistrationSource());
        }

        private static void StartStartableComponents(IComponentContext componentContext)
        {
            foreach (IComponentRegistration registration in componentContext.ComponentRegistry.RegistrationsFor(new TypedService(typeof(IStartable))))
            {
                ((IStartable) componentContext.ResolveComponent(registration, Enumerable.Empty<Parameter>())).Start();
            }
        }

        public void Update(IComponentRegistry componentRegistry)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            this.Build(componentRegistry, true);
        }

        public void Update(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Update(container.ComponentRegistry);
        }
    }
}


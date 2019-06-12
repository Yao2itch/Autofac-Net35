namespace Autofac.Core
{
    using Autofac.Util;
    using System;

    public class ComponentRegisteredEventArgs : EventArgs
    {
        private readonly IComponentRegistry _componentRegistry;
        private readonly IComponentRegistration _componentRegistration;

        public ComponentRegisteredEventArgs(IComponentRegistry registry, IComponentRegistration componentRegistration)
        {
            this._componentRegistry = Enforce.ArgumentNotNull<IComponentRegistry>(registry, "registry");
            this._componentRegistration = Enforce.ArgumentNotNull<IComponentRegistration>(componentRegistration, "componentRegistration");
        }

        public IComponentRegistry ComponentRegistry
        {
            get
            {
                return this._componentRegistry;
            }
        }

        public IComponentRegistration ComponentRegistration
        {
            get
            {
                return this._componentRegistration;
            }
        }
    }
}


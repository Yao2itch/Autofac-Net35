namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using Autofac.Core.Lifetime;
    using System;

    internal class ScopeRestrictedRegistry : ComponentRegistry
    {
        private readonly IComponentLifetime _restrictedRootScopeLifetime;

        public ScopeRestrictedRegistry(object scopeTag)
        {
            this._restrictedRootScopeLifetime = new MatchingScopeLifetime(scopeTag);
        }

        public override void Register(IComponentRegistration registration, bool preserveDefaults)
        {
            IComponentRegistration registration2 = registration;
            if (registration.Lifetime is RootScopeLifetime)
            {
                registration2 = new ComponentRegistrationLifetimeDecorator(registration, this._restrictedRootScopeLifetime);
            }
            base.Register(registration2, preserveDefaults);
        }
    }
}


namespace Autofac.Core.Lifetime
{
    using Autofac;
    using System;

    public class LifetimeScopeEndingEventArgs : EventArgs
    {
        private readonly ILifetimeScope _lifetimeScope;

        public LifetimeScopeEndingEventArgs(ILifetimeScope lifetimeScope)
        {
            this._lifetimeScope = lifetimeScope;
        }

        public ILifetimeScope LifetimeScope
        {
            get
            {
                return this._lifetimeScope;
            }
        }
    }
}


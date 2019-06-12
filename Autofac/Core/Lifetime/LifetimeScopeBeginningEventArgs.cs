namespace Autofac.Core.Lifetime
{
    using Autofac;
    using System;

    public class LifetimeScopeBeginningEventArgs : EventArgs
    {
        private readonly ILifetimeScope _lifetimeScope;

        public LifetimeScopeBeginningEventArgs(ILifetimeScope lifetimeScope)
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


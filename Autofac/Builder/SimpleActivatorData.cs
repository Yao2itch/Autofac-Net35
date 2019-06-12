namespace Autofac.Builder
{
    using Autofac.Core;
    using Autofac.Util;
    using System;

    public class SimpleActivatorData : IConcreteActivatorData
    {
        private readonly IInstanceActivator _activator;

        public SimpleActivatorData(IInstanceActivator activator)
        {
            this._activator = Enforce.ArgumentNotNull<IInstanceActivator>(activator, "activator");
        }

        public IInstanceActivator Activator
        {
            get
            {
                return this._activator;
            }
        }
    }
}


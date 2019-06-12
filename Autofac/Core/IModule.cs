namespace Autofac.Core
{
    using System;

    public interface IModule
    {
        void Configure(IComponentRegistry componentRegistry);
    }
}


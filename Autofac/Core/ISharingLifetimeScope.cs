namespace Autofac.Core
{
    using Autofac;
    using System;

    public interface ISharingLifetimeScope : ILifetimeScope, IComponentContext, IDisposable
    {
        object GetOrCreateAndShare(Guid id, Func<object> creator);

        ISharingLifetimeScope RootLifetimeScope { get; }

        ISharingLifetimeScope ParentLifetimeScope { get; }
    }
}


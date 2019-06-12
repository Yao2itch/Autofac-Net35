namespace Autofac
{
    using Autofac.Core;
    using Core.Resolving;
    using Core.Lifetime;
    using System;

    public interface ILifetimeScope : IComponentContext, IDisposable
    {
        event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning;

        event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding;

        event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning;

        ILifetimeScope BeginLifetimeScope();
        ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction);
        ILifetimeScope BeginLifetimeScope(object tag);
        ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction);

        IDisposer Disposer { get; }

        object Tag { get; }
    }
}


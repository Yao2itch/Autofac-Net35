namespace Autofac
{
    using System;

    public interface IContainer : ILifetimeScope, IComponentContext, IDisposable
    {
    }
}


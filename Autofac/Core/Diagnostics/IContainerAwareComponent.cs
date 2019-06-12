namespace Autofac.Core.Diagnostics
{
    using Autofac;
    using System;

    [Obsolete("Use the more general Autofac.IStartable interface instead. The IContainer parameter can be emulated when implementing IStartable by taking a dependency on IComponentContext or ILifetimeScope.", true)]
    public interface IContainerAwareComponent
    {
        void SetContainer(IContainer container);
    }
}


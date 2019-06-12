namespace Autofac.Core
{
    using Autofac;
    using System;
    using System.Collections.Generic;

    public interface IInstanceActivator : IDisposable
    {
        object ActivateInstance(IComponentContext context, IEnumerable<Parameter> parameters);

        Type LimitType { get; }
    }
}


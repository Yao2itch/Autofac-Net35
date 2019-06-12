namespace Autofac.Core.Resolving
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;

    public interface IInstanceLookup
    {
        event EventHandler<InstanceLookupCompletionBeginningEventArgs> CompletionBeginning;

        event EventHandler<InstanceLookupCompletionEndingEventArgs> CompletionEnding;

        event EventHandler<InstanceLookupEndingEventArgs> InstanceLookupEnding;

        IComponentRegistration ComponentRegistration { get; }

        ILifetimeScope ActivationScope { get; }

        IEnumerable<Parameter> Parameters { get; }
    }
}


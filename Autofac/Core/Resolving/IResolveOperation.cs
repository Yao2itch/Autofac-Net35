namespace Autofac.Core.Resolving
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;

    public interface IResolveOperation
    {
        event EventHandler<ResolveOperationEndingEventArgs> CurrentOperationEnding;

        event EventHandler<InstanceLookupBeginningEventArgs> InstanceLookupBeginning;

        object GetOrCreateInstance(ISharingLifetimeScope currentOperationScope, IComponentRegistration registration, IEnumerable<Parameter> parameters);
    }
}


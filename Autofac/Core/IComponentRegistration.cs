namespace Autofac.Core
{
    using Autofac;
    using System;
    using System.Collections.Generic;

    public interface IComponentRegistration : IDisposable
    {
        event EventHandler<ActivatedEventArgs<object>> Activated;

        event EventHandler<ActivatingEventArgs<object>> Activating;

        event EventHandler<PreparingEventArgs> Preparing;

        void RaiseActivated(IComponentContext context, IEnumerable<Parameter> parameters, object instance);
        void RaiseActivating(IComponentContext context, IEnumerable<Parameter> parameters, ref object instance);
        void RaisePreparing(IComponentContext context, ref IEnumerable<Parameter> parameters);

        Guid Id { get; }

        IInstanceActivator Activator { get; }

        IComponentLifetime Lifetime { get; }

        InstanceSharing Sharing { get; }

        InstanceOwnership Ownership { get; }

        IEnumerable<Service> Services { get; }

        IDictionary<string, object> Metadata { get; }

        IComponentRegistration Target { get; }
    }
}


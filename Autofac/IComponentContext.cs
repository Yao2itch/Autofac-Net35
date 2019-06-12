namespace Autofac
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;

    public interface IComponentContext
    {
        object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters);

        IComponentRegistry ComponentRegistry { get; }
    }
}


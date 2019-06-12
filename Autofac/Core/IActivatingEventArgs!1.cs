namespace Autofac.Core
{
    using Autofac;
    using System;
    using System.Collections.Generic;

    public interface IActivatingEventArgs<T>
    {
        void ReplaceInstance(object instance);

        IComponentContext Context { get; }

        IComponentRegistration Component { get; }

        T Instance { get; }

        IEnumerable<Parameter> Parameters { get; }
    }
}


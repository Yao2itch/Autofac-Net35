namespace Autofac.Core
{
    using Autofac;
    using System.Collections.Generic;

    public interface IActivatedEventArgs<T>
    {
        IComponentContext Context { get; }

        IComponentRegistration Component { get; }

        IEnumerable<Parameter> Parameters { get; }

        T Instance { get; }
    }
}


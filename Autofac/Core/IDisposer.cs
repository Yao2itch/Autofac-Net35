namespace Autofac.Core
{
    using System;

    public interface IDisposer : IDisposable
    {
        void AddInstanceForDisposal(IDisposable instance);
    }
}


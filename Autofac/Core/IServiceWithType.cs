namespace Autofac.Core
{
    using System;

    public interface IServiceWithType
    {
        Service ChangeType(Type newType);

        Type ServiceType { get; }
    }
}


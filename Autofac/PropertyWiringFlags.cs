namespace Autofac
{
    using System;

    [Flags]
    public enum PropertyWiringFlags
    {
        Default,
        AllowCircularDependencies,
        PreserveSetValues
    }
}


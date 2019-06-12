namespace Autofac.Builder
{
    using System;

    [Flags]
    public enum ContainerBuildOptions
    {
        None = 0,
        Default = 0,
        ExcludeDefaultModules = 2,
        IgnoreStartableComponents = 4
    }
}


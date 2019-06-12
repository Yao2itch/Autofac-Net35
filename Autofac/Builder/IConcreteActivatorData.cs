namespace Autofac.Builder
{
    using Autofac.Core;

    public interface IConcreteActivatorData
    {
        IInstanceActivator Activator { get; }
    }
}


namespace Autofac.Builder
{
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using System;

    public class ConcreteReflectionActivatorData : ReflectionActivatorData, IConcreteActivatorData
    {
        public ConcreteReflectionActivatorData(Type implementor) : base(implementor)
        {
        }

        public IInstanceActivator Activator
        {
            get
            {
                return new ReflectionActivator(base.ImplementationType, base.ConstructorFinder, base.ConstructorSelector, base.ConfiguredParameters, base.ConfiguredProperties);
            }
        }
    }
}


namespace Autofac.Core.Activators.Reflection
{
    using System;
    using System.Reflection;

    public interface IConstructorFinder
    {
        ConstructorInfo[] FindConstructors(Type targetType);
    }
}


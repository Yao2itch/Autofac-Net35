namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using System;
    using System.Runtime.CompilerServices;

    internal static class ComponentRegistrationExtensions
    {
        public static bool IsAdapting(this IComponentRegistration componentRegistration)
        {
            return (componentRegistration.Target != componentRegistration);
        }
    }
}


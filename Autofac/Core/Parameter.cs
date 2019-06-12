namespace Autofac.Core
{
    using Autofac;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public abstract class Parameter
    {
        protected Parameter()
        {
        }

        public abstract bool CanSupplyValue(ParameterInfo pi, IComponentContext context, out Func<object> valueProvider);
    }
}


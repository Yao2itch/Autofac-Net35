namespace Autofac.Core.Activators.Reflection
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class DefaultValueParameter : Parameter
    {
        public override bool CanSupplyValue(ParameterInfo pi, IComponentContext context, out Func<object> valueProvider)
        {
            Func<object> func = null;
            if (!(pi.DefaultValue is DBNull))
            {
                if (func == null)
                {
                    func = () => pi.DefaultValue;
                }
                valueProvider = func;
                return true;
            }
            valueProvider = null;
            return false;
        }
    }
}


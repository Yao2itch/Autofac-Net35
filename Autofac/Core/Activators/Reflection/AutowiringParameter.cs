namespace Autofac.Core.Activators.Reflection
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class AutowiringParameter : Parameter
    {
        public override bool CanSupplyValue(ParameterInfo pi, IComponentContext context, out Func<object> valueProvider)
        {
            IComponentRegistration registration;
            Func<object> func = null;
            if (context.ComponentRegistry.TryGetRegistration(new TypedService(pi.ParameterType), out registration))
            {
                if (func == null)
                {
                    func = () => context.ResolveComponent(registration, Enumerable.Empty<Parameter>());
                }
                valueProvider = func;
                return true;
            }
            valueProvider = null;
            return false;
        }
    }
}


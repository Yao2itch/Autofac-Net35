namespace Autofac.Util.WindowsPhone
{
    using Autofac;
    using Core;
    using Autofac.Features.GeneratedFactories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class Wp7Extensions
    {
        internal static IEnumerable<Parameter> GetParameterCollection<TDelegate>(this ParameterMapping mapping, params object[] param)
        {
            IEnumerable<Parameter> enumerable;
            Func<object, Parameter> selector = null;
            switch (mapping)
            {
                case ParameterMapping.ByName:
                {
                    ParameterInfo[] parameters = typeof(TDelegate).GetMethods().First<MethodInfo>().GetParameters();
                    enumerable = new List<Parameter>();
                    for (int i = 0; i < param.Length; i++)
                    {
                        ((IList<Parameter>) enumerable).Add(new NamedParameter(parameters[i].Name, param[i]));
                    }
                    return enumerable;
                }
                case ParameterMapping.ByType:
                    if (selector == null)
                    {
                        selector = x => new TypedParameter(x.GetType(), x);
                    }
                    return param.Select<object, Parameter>(selector);

                case ParameterMapping.ByPosition:
                    enumerable = new List<Parameter>();
                    for (int i = 0; i < param.Length; i++)
                    {
                        ((IList<Parameter>) enumerable).Add(new PositionalParameter(i, param[i]));
                    }
                    return enumerable;
            }
            throw new NotSupportedException("Parameter mapping not supported");
        }

        internal static ParameterMapping ResolveParameterMapping(this ParameterMapping configuredParameterMapping, Type delegateType)
        {
            if (configuredParameterMapping != ParameterMapping.Adaptive)
            {
                return configuredParameterMapping;
            }
            if (!delegateType.Name.StartsWith("Func`"))
            {
                return ParameterMapping.ByName;
            }
            return ParameterMapping.ByType;
        }
    }
}


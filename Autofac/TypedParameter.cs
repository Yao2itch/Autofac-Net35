namespace Autofac
{
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TypedParameter : ConstantParameter
    {
        public TypedParameter(System.Type type, object value) : base(value, pi => pi.ParameterType == type)
        {
            Predicate<ParameterInfo> predicate = null;
            if (predicate == null)
            {
                predicate = pi => pi.ParameterType == type;
            }
            this.Type = Enforce.ArgumentNotNull<System.Type>(type, "type");
        }

        public static TypedParameter From<T>(T value)
        {
            return new TypedParameter(typeof(T), value);
        }

        public System.Type Type { get; private set; }
    }
}


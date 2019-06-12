namespace Autofac.Core
{
    using Autofac.Util;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class NamedPropertyParameter : ConstantParameter
    {
        public NamedPropertyParameter(string name, object value) : base(value, pi =>
        {
            PropertyInfo prop;
            return pi.TryGetDeclaringProperty(out prop) &&
                prop.Name == name;
        })
        {
            Predicate<ParameterInfo> predicate = null;
            if (predicate == null)
            {
                predicate = delegate (ParameterInfo pi) {
                    PropertyInfo info;
                    return pi.TryGetDeclaringProperty(out info) && (info.Name == name);
                };
            }
            this.Name = Enforce.ArgumentNotNullOrEmpty(name, "name");
        }

        public string Name { get; private set; }
    }
}


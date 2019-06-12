namespace Autofac
{
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class NamedParameter : ConstantParameter
    {
        public NamedParameter(string name, object value) : base(value, pi => pi.Name == name)
        {
            Predicate<ParameterInfo> predicate = null;
            if (predicate == null)
            {
                predicate = pi => pi.Name == name;
            }
            this.Name = Enforce.ArgumentNotNullOrEmpty(name, "name");
        }

        public string Name { get; private set; }
    }
}


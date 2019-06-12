namespace Autofac
{
    using Autofac.Core;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class PositionalParameter : ConstantParameter
    {
        public PositionalParameter(int position, object value) : base(value, pi => pi.Position == position &&
        (pi.Member is ConstructorInfo))
        {
            Predicate<ParameterInfo> predicate = null;
            if (predicate == null)
            {
                predicate = pi => (pi.Position == position) && (pi.Member is ConstructorInfo);
            }
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException("position");
            }
            this.Position = position;
        }

        public int Position { get; private set; }
    }
}


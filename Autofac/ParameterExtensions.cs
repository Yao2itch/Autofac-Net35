namespace Autofac
{
    using Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class ParameterExtensions
    {
        private static TValue ConstantValue<TParameter, TValue>(IEnumerable<Parameter> parameters, Func<TParameter, bool> predicate) where TParameter: ConstantParameter
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            return (from p in parameters.OfType<TParameter>().Where<TParameter>(predicate) select p.Value).Cast<TValue>().First<TValue>();
        }

        public static T Named<T>(this IEnumerable<Parameter> parameters, string name)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            Enforce.ArgumentNotNullOrEmpty(name, "name");
            return ConstantValue<NamedParameter, T>(parameters, c => c.Name == name);
        }

        public static T Positional<T>(this IEnumerable<Parameter> parameters, int position)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException("position");
            }
            return ConstantValue<PositionalParameter, T>(parameters, c => c.Position == position);
        }

        public static T TypedAs<T>(this IEnumerable<Parameter> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            return ConstantValue<TypedParameter, T>(parameters, c => c.Type == typeof(T));
        }
    }
}


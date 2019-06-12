namespace Autofac.Core
{
    using Autofac;
    using Autofac.Util;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class ConstantParameter : Parameter
    {
        private Predicate<ParameterInfo> _predicate;

        protected ConstantParameter(object value, Predicate<ParameterInfo> predicate)
        {
            this.Value = value;
            this._predicate = Enforce.ArgumentNotNull<Predicate<ParameterInfo>>(predicate, "predicate");
        }

        public override bool CanSupplyValue(ParameterInfo pi, IComponentContext context, out Func<object> valueProvider)
        {
            Func<object> func = null;
            if (pi == null)
            {
                throw new ArgumentNullException("pi");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (this._predicate(pi))
            {
                if (func == null)
                {
                    func = () => this.Value;
                }
                valueProvider = func;
                return true;
            }
            valueProvider = null;
            return false;
        }

        public object Value { get; private set; }
    }
}


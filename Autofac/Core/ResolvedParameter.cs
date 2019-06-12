namespace Autofac.Core
{
    using Autofac;
    using Autofac.Util;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class ResolvedParameter : Parameter
    {
        private readonly Func<ParameterInfo, IComponentContext, bool> _predicate;
        private readonly Func<ParameterInfo, IComponentContext, object> _valueAccessor;

        public ResolvedParameter(Func<ParameterInfo, IComponentContext, bool> predicate, Func<ParameterInfo, IComponentContext, object> valueAccessor)
        {
            this._predicate = Enforce.ArgumentNotNull<Func<ParameterInfo, IComponentContext, bool>>(predicate, "predicate");
            this._valueAccessor = Enforce.ArgumentNotNull<Func<ParameterInfo, IComponentContext, object>>(valueAccessor, "valueAccessor");
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
            if (this._predicate(pi, context))
            {
                if (func == null)
                {
                    func = () => this._valueAccessor(pi, context);
                }
                valueProvider = func;
                return true;
            }
            valueProvider = null;
            return false;
        }

        public static ResolvedParameter ForKeyed<TService>(object serviceKey)
        {
            if (serviceKey == null)
            {
                throw new ArgumentNullException("serviceKey");
            }
            KeyedService ks = new KeyedService(serviceKey, typeof(TService));
            return new ResolvedParameter((pi, c) => (pi.ParameterType == typeof(TService)) && c.IsRegisteredService(ks), (pi, c) => c.ResolveService(ks));
        }

        public static ResolvedParameter ForNamed<TService>(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }
            return ForKeyed<TService>(serviceName);
        }
    }
}


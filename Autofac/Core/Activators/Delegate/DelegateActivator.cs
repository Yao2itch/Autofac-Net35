namespace Autofac.Core.Activators.Delegate
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class DelegateActivator : InstanceActivator, IInstanceActivator, IDisposable
    {
        private readonly Func<IComponentContext, IEnumerable<Parameter>, object> _activationFunction;

        public DelegateActivator(Type limitType, Func<IComponentContext, IEnumerable<Parameter>, object> activationFunction) : base(limitType)
        {
            this._activationFunction = Enforce.ArgumentNotNull<Func<IComponentContext, IEnumerable<Parameter>, object>>(activationFunction, "activationFunction");
        }

        public object ActivateInstance(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            object obj2 = this._activationFunction(context, parameters);
            if (obj2 == null)
            {
                throw new DependencyResolutionException(string.Format(DelegateActivatorResources.NullFromActivationDelegateFor, base.LimitType));
            }
            return obj2;
        }
    }
}


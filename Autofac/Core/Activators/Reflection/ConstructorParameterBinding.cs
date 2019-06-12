namespace Autofac.Core.Activators.Reflection
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ConstructorParameterBinding
    {
        private readonly ConstructorInfo _ci;
        private readonly Func<object>[] _valueRetrievers;
        private readonly bool _canInstantiate = true;
        private readonly ParameterInfo _firstNonBindableParameter;

        public ConstructorParameterBinding(ConstructorInfo ci, IEnumerable<Parameter> availableParameters, IComponentContext context)
        {
            this._ci = Enforce.ArgumentNotNull<ConstructorInfo>(ci, "ci");
            if (availableParameters == null)
            {
                throw new ArgumentNullException("availableParameters");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            ParameterInfo[] parameters = ci.GetParameters();
            this._valueRetrievers = new Func<object>[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo pi = parameters[i];
                bool flag = false;
                foreach (Parameter parameter in availableParameters)
                {
                    Func<object> func;
                    if (parameter.CanSupplyValue(pi, context, out func))
                    {
                        this._valueRetrievers[i] = func;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this._canInstantiate = false;
                    this._firstNonBindableParameter = pi;
                    return;
                }
            }
        }

        public object Instantiate()
        {
            object obj2;
            if (!this.CanInstantiate)
            {
                throw new InvalidOperationException(ConstructorParameterBindingResources.CannotInstantitate);
            }
            object[] parameters = new object[this._valueRetrievers.Length];
            for (int i = 0; i < this._valueRetrievers.Length; i++)
            {
                parameters[i] = this._valueRetrievers[i]();
            }
            try
            {
                obj2 = this.TargetConstructor.Invoke(parameters);
            }
            catch (TargetInvocationException exception)
            {
                throw new DependencyResolutionException(string.Format(ConstructorParameterBindingResources.ExceptionDuringInstantiation, this.TargetConstructor, this.TargetConstructor.DeclaringType.Name), exception.InnerException);
            }
            catch (Exception exception2)
            {
                throw new DependencyResolutionException(string.Format(ConstructorParameterBindingResources.ExceptionDuringInstantiation, this.TargetConstructor, this.TargetConstructor.DeclaringType.Name), exception2);
            }
            return obj2;
        }

        public override string ToString()
        {
            return this.Description;
        }

        public ConstructorInfo TargetConstructor
        {
            get
            {
                return this._ci;
            }
        }

        public bool CanInstantiate
        {
            get
            {
                return this._canInstantiate;
            }
        }

        public string Description
        {
            get
            {
                if (!this.CanInstantiate)
                {
                    return string.Format(ConstructorParameterBindingResources.NonBindableConstructor, this._ci, this._firstNonBindableParameter);
                }
                return string.Format(ConstructorParameterBindingResources.BoundConstructor, this._ci);
            }
        }
    }
}


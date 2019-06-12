namespace Autofac.Core.Activators.Reflection
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class ReflectionActivator : InstanceActivator, IInstanceActivator, IDisposable
    {
        private readonly Type _implementationType;
        private readonly IConstructorSelector _constructorSelector;
        private readonly IConstructorFinder _constructorFinder;
        private readonly IEnumerable<Parameter> _configuredParameters;
        private readonly IEnumerable<Parameter> _configuredProperties;

        public ReflectionActivator(Type implementationType, IConstructorFinder constructorFinder, IConstructorSelector constructorSelector, IEnumerable<Parameter> configuredParameters, IEnumerable<Parameter> configuredProperties) : base(Enforce.ArgumentNotNull<Type>(implementationType, "implementationType"))
        {
            this._implementationType = implementationType;
            this._constructorFinder = Enforce.ArgumentNotNull<IConstructorFinder>(constructorFinder, "constructorFinder");
            this._constructorSelector = Enforce.ArgumentNotNull<IConstructorSelector>(constructorSelector, "constructorSelector");
            this._configuredParameters = Enforce.ArgumentNotNull<IEnumerable<Parameter>>(configuredParameters, "configuredParameters");
            this._configuredProperties = Enforce.ArgumentNotNull<IEnumerable<Parameter>>(configuredProperties, "configuredProperties");
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
            ConstructorInfo[] constructorInfo = this._constructorFinder.FindConstructors(this._implementationType);
            if (constructorInfo.Length == 0)
            {
                throw new DependencyResolutionException(string.Format(ReflectionActivatorResources.NoConstructorsAvailable, this._implementationType, this._constructorFinder));
            }
            IEnumerable<ConstructorParameterBinding> constructorBindings = this.GetConstructorBindings(context, parameters, constructorInfo);
            ConstructorParameterBinding[] bindingArray = (from cb in constructorBindings
                where cb.CanInstantiate
                select cb).ToArray<ConstructorParameterBinding>();
            if (bindingArray.Length == 0)
            {
                throw new DependencyResolutionException(this.GetBindingFailureMessage(constructorBindings));
            }
            object instance = this._constructorSelector.SelectConstructorBinding(bindingArray).Instantiate();
            this.InjectProperties(instance, context);
            return instance;
        }

        private string GetBindingFailureMessage(IEnumerable<ConstructorParameterBinding> constructorBindings)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ConstructorParameterBinding binding in from cb in constructorBindings
                where !cb.CanInstantiate
                select cb)
            {
                builder.AppendLine();
                builder.Append(binding.Description);
            }
            return string.Format(ReflectionActivatorResources.NoConstructorsBindable, this._constructorFinder, this._implementationType, builder);
        }

        private IEnumerable<ConstructorParameterBinding> GetConstructorBindings(IComponentContext context, IEnumerable<Parameter> parameters, IEnumerable<ConstructorInfo> constructorInfo)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }
            IEnumerable<Parameter> prioritisedParameters = parameters.Concat<Parameter>(this._configuredParameters.Concat<Parameter>(new Parameter[] { new AutowiringParameter(), new DefaultValueParameter() }));
            return (from ci in constructorInfo select new ConstructorParameterBinding(ci, prioritisedParameters, context));
        }

        private void InjectProperties(object instance, IComponentContext context)
        {
            if (this._configuredProperties.Any<Parameter>())
            {
                List<PropertyInfo> list = (from pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where pi.CanWrite
                    select pi).ToList<PropertyInfo>();
                foreach (Parameter parameter in this._configuredProperties)
                {
                    foreach (PropertyInfo info in list)
                    {
                        Func<object> func;
                        MethodInfo setMethod = info.GetSetMethod();
                        if ((setMethod != null) && parameter.CanSupplyValue(setMethod.GetParameters().First<ParameterInfo>(), context, out func))
                        {
                            list.Remove(info);
                            info.SetValue(instance, func(), null);
                            break;
                        }
                    }
                }
            }
        }

        public IConstructorFinder ConstructorFinder
        {
            get
            {
                return this._constructorFinder;
            }
        }

        public IConstructorSelector ConstructorSelector
        {
            get
            {
                return this._constructorSelector;
            }
        }
    }
}


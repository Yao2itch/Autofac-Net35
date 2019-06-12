namespace Autofac.Builder
{
    using Core;
    using Autofac.Core.Activators.Reflection;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ReflectionActivatorData
    {
        private Type _implementor;
        private IConstructorFinder _constructorFinder = new BindingFlagsConstructorFinder(BindingFlags.Public);
        private IConstructorSelector _constructorSelector = new MostParametersConstructorSelector();
        private readonly IList<Parameter> _configuredParameters = new List<Parameter>();
        private readonly IList<Parameter> _configuredProperties = new List<Parameter>();

        public ReflectionActivatorData(Type implementor)
        {
            this.ImplementationType = implementor;
        }

        public Type ImplementationType
        {
            get
            {
                return this._implementor;
            }
            set
            {
                this._implementor = Enforce.ArgumentNotNull<Type>(value, "value");
            }
        }

        public IConstructorFinder ConstructorFinder
        {
            get
            {
                return this._constructorFinder;
            }
            set
            {
                this._constructorFinder = Enforce.ArgumentNotNull<IConstructorFinder>(value, "value");
            }
        }

        public IConstructorSelector ConstructorSelector
        {
            get
            {
                return this._constructorSelector;
            }
            set
            {
                this._constructorSelector = Enforce.ArgumentNotNull<IConstructorSelector>(value, "value");
            }
        }

        public IList<Parameter> ConfiguredParameters
        {
            get
            {
                return this._configuredParameters;
            }
        }

        public IList<Parameter> ConfiguredProperties
        {
            get
            {
                return this._configuredProperties;
            }
        }
    }
}


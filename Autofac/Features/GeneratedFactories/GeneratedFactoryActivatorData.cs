namespace Autofac.Features.GeneratedFactories
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class GeneratedFactoryActivatorData : IConcreteActivatorData
    {
        private Autofac.Features.GeneratedFactories.ParameterMapping _parameterMapping;
        private Type _delegateType;
        private Service _productService;

        public GeneratedFactoryActivatorData(Type delegateType, Service productService)
        {
            this._delegateType = Enforce.ArgumentNotNull<Type>(delegateType, "delegateType");
            this._productService = Enforce.ArgumentNotNull<Service>(productService, "productService");
        }

        public Autofac.Features.GeneratedFactories.ParameterMapping ParameterMapping
        {
            get
            {
                return this._parameterMapping;
            }
            set
            {
                this._parameterMapping = value;
            }
        }

        public IInstanceActivator Activator
        {
            get
            {
                FactoryGenerator factory = new FactoryGenerator(this._delegateType, this._productService, this.ParameterMapping);
                return new DelegateActivator(this._delegateType, (c, p) => factory.GenerateFactory(c, p));
            }
        }
    }
}


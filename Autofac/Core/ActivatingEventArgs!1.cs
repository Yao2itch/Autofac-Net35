namespace Autofac.Core
{
    using Autofac;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class ActivatingEventArgs<T> : EventArgs, IActivatingEventArgs<T>
    {
        private readonly IComponentContext _context;
        private readonly IComponentRegistration _component;
        private T _instance;
        private readonly IEnumerable<Parameter> _parameters;

        public ActivatingEventArgs(IComponentContext context, IComponentRegistration component, IEnumerable<Parameter> parameters, T instance)
        {
            this._context = Enforce.ArgumentNotNull<IComponentContext>(context, "context");
            this._component = Enforce.ArgumentNotNull<IComponentRegistration>(component, "component");
            this._parameters = Enforce.ArgumentNotNull<IEnumerable<Parameter>>(parameters, "parameters");
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            this._instance = instance;
        }

        void IActivatingEventArgs<T>.ReplaceInstance(object instance)
        {
            this.Instance = (T) instance;
        }

        public IComponentContext Context
        {
            get
            {
                return this._context;
            }
        }

        public IComponentRegistration Component
        {
            get
            {
                return this._component;
            }
        }

        public T Instance
        {
            get
            {
                return this._instance;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._instance = value;
            }
        }

        public IEnumerable<Parameter> Parameters
        {
            get
            {
                return this._parameters;
            }
        }
    }
}


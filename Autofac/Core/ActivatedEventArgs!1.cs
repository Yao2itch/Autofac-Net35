namespace Autofac.Core
{
    using Autofac;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class ActivatedEventArgs<T> : EventArgs, IActivatedEventArgs<T>
    {
        private readonly IComponentContext _context;
        private readonly IComponentRegistration _component;
        private readonly IEnumerable<Parameter> _parameters;
        private readonly T _instance;

        public ActivatedEventArgs(IComponentContext context, IComponentRegistration component, IEnumerable<Parameter> parameters, T instance)
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

        public IEnumerable<Parameter> Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public T Instance
        {
            get
            {
                return this._instance;
            }
        }
    }
}


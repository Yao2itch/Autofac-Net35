namespace Autofac.Core
{
    using Autofac;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    public class PreparingEventArgs : EventArgs
    {
        private readonly IComponentContext _context;
        private readonly IComponentRegistration _component;
        private IEnumerable<Parameter> _parameters;

        public PreparingEventArgs(IComponentContext context, IComponentRegistration component, IEnumerable<Parameter> parameters)
        {
            this._context = Enforce.ArgumentNotNull<IComponentContext>(context, "context");
            this._component = Enforce.ArgumentNotNull<IComponentRegistration>(component, "component");
            this._parameters = Enforce.ArgumentNotNull<IEnumerable<Parameter>>(parameters, "parameters");
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
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this._parameters = value;
            }
        }
    }
}


namespace Autofac.Builder
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SingleRegistrationStyle
    {
        private Guid _id = Guid.NewGuid();
        private readonly ICollection<EventHandler<ComponentRegisteredEventArgs>> _registeredHandlers = new List<EventHandler<ComponentRegisteredEventArgs>>();
        private bool _preserveDefaults;

        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public ICollection<EventHandler<ComponentRegisteredEventArgs>> RegisteredHandlers
        {
            get
            {
                return this._registeredHandlers;
            }
        }

        public bool PreserveDefaults
        {
            get
            {
                return this._preserveDefaults;
            }
            set
            {
                this._preserveDefaults = value;
            }
        }

        public IComponentRegistration Target { get; set; }
    }
}


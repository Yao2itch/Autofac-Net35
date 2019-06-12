namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class CopyOnWriteRegistry : IComponentRegistry, IDisposable
    {
        private readonly IComponentRegistry _readRegistry;
        private readonly Func<IComponentRegistry> _createWriteRegistry;
        private IComponentRegistry _writeRegistry;

        public event EventHandler<ComponentRegisteredEventArgs> Registered
        {
            add
            {
                this.WriteRegistry.Registered += value;
            }
            remove
            {
                this.WriteRegistry.Registered -= value;
            }
        }

        public event EventHandler<RegistrationSourceAddedEventArgs> RegistrationSourceAdded
        {
            add
            {
                this.WriteRegistry.RegistrationSourceAdded += value;
            }
            remove
            {
                this.WriteRegistry.RegistrationSourceAdded -= value;
            }
        }

        public CopyOnWriteRegistry(IComponentRegistry readRegistry, Func<IComponentRegistry> createWriteRegistry)
        {
            if (readRegistry == null)
            {
                throw new ArgumentNullException("readRegistry");
            }
            if (createWriteRegistry == null)
            {
                throw new ArgumentNullException("createWriteRegistry");
            }
            this._readRegistry = readRegistry;
            this._createWriteRegistry = createWriteRegistry;
        }

        public void AddRegistrationSource(IRegistrationSource source)
        {
            this.WriteRegistry.AddRegistrationSource(source);
        }

        public void Dispose()
        {
            if (this._readRegistry != null)
            {
                this._readRegistry.Dispose();
            }
        }

        public bool IsRegistered(Service service)
        {
            return this.Registry.IsRegistered(service);
        }

        public void Register(IComponentRegistration registration)
        {
            this.WriteRegistry.Register(registration);
        }

        public void Register(IComponentRegistration registration, bool preserveDefaults)
        {
            this.WriteRegistry.Register(registration, preserveDefaults);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service)
        {
            return this.Registry.RegistrationsFor(service);
        }

        public bool TryGetRegistration(Service service, out IComponentRegistration registration)
        {
            return this.Registry.TryGetRegistration(service, out registration);
        }

        private IComponentRegistry Registry
        {
            get
            {
                return (this._writeRegistry ?? this._readRegistry);
            }
        }

        private IComponentRegistry WriteRegistry
        {
            get
            {
                if (this._writeRegistry == null)
                {
                    this._writeRegistry = this._createWriteRegistry();
                }
                return this._writeRegistry;
            }
        }

        public IEnumerable<IComponentRegistration> Registrations
        {
            get
            {
                return this.Registry.Registrations;
            }
        }

        public IEnumerable<IRegistrationSource> Sources
        {
            get
            {
                return this.Registry.Sources;
            }
        }

        public bool HasLocalComponents
        {
            get
            {
                return (this._writeRegistry != null);
            }
        }
    }
}


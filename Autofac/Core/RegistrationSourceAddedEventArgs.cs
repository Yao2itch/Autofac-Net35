namespace Autofac.Core
{
    using System;

    public class RegistrationSourceAddedEventArgs : EventArgs
    {
        private readonly IComponentRegistry _componentRegistry;
        private readonly IRegistrationSource _registrationSource;

        public RegistrationSourceAddedEventArgs(IComponentRegistry componentRegistry, IRegistrationSource registrationSource)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            if (registrationSource == null)
            {
                throw new ArgumentNullException("registrationSource");
            }
            this._componentRegistry = componentRegistry;
            this._registrationSource = registrationSource;
        }

        public IRegistrationSource RegistrationSource
        {
            get
            {
                return this._registrationSource;
            }
        }

        public IComponentRegistry ComponentRegistry
        {
            get
            {
                return this._componentRegistry;
            }
        }
    }
}


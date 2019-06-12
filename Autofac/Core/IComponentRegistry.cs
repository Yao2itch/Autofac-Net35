namespace Autofac.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface IComponentRegistry : IDisposable
    {
        event EventHandler<ComponentRegisteredEventArgs> Registered;

        event EventHandler<RegistrationSourceAddedEventArgs> RegistrationSourceAdded;

        void AddRegistrationSource(IRegistrationSource source);
        bool IsRegistered(Service service);
        void Register(IComponentRegistration registration);
        void Register(IComponentRegistration registration, bool preserveDefaults);
        IEnumerable<IComponentRegistration> RegistrationsFor(Service service);
        bool TryGetRegistration(Service service, out IComponentRegistration registration);

        IEnumerable<IComponentRegistration> Registrations { get; }

        IEnumerable<IRegistrationSource> Sources { get; }

        bool HasLocalComponents { get; }
    }
}


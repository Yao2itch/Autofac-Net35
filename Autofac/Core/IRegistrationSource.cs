namespace Autofac.Core
{
    using System;
    using System.Collections.Generic;

    public interface IRegistrationSource
    {
        IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor);

        bool IsAdapterForIndividualComponents { get; }
    }
}


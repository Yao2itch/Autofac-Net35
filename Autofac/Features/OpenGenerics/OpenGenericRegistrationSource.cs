namespace Autofac.Features.OpenGenerics
{
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class OpenGenericRegistrationSource : IRegistrationSource
    {
        private readonly RegistrationData _registrationData;
        private readonly ReflectionActivatorData _activatorData;

        public OpenGenericRegistrationSource(RegistrationData registrationData, ReflectionActivatorData activatorData)
        {
            if (registrationData == null)
            {
                throw new ArgumentNullException("registrationData");
            }
            if (activatorData == null)
            {
                throw new ArgumentNullException("activatorData");
            }
            OpenGenericServiceBinder.EnforceBindable(activatorData.ImplementationType, registrationData.Services);
            this._registrationData = registrationData;
            this._activatorData = activatorData;
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (registrationAccessor == null) throw new ArgumentNullException("registrationAccessor");

            Type constructedImplementationType;
            IEnumerable<Service> services;
            if (OpenGenericServiceBinder.TryBindServiceType(service, _registrationData.Services, _activatorData.ImplementationType, out constructedImplementationType, out services))
            {
                yield return RegistrationBuilder.CreateRegistration(
                    Guid.NewGuid(),
                    _registrationData,
                    new ReflectionActivator(
                        constructedImplementationType,
                        _activatorData.ConstructorFinder,
                        _activatorData.ConstructorSelector,
                        _activatorData.ConfiguredParameters,
                        _activatorData.ConfiguredProperties),
                    services);
            }

            //return new <RegistrationsFor>d__0(-2) { 
            //    <>4__this = this,
            //    <>3__service = service,
            //    <>3__registrationAccessor = registrationAccessor
            //};
        }

        public override string ToString()
        {
            return string.Format(OpenGenericRegistrationSourceResources.OpenGenericRegistrationSourceDescription, this._activatorData.ImplementationType.FullName, string.Join(", ", (from s in this._registrationData.Services select s.Description).ToArray<string>()));
        }

        public bool IsAdapterForIndividualComponents
        {
            get
            {
                return false;
            }
        }
    }
}


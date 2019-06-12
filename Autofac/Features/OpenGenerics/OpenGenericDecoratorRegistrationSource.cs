namespace Autofac.Features.OpenGenerics
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class OpenGenericDecoratorRegistrationSource : IRegistrationSource
    {
        private readonly RegistrationData _registrationData;
        private readonly OpenGenericDecoratorActivatorData _activatorData;

        public OpenGenericDecoratorRegistrationSource(RegistrationData registrationData, OpenGenericDecoratorActivatorData activatorData)
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
            if (registrationData.Services.Contains<Service>((Service) activatorData.FromService))
            {
                throw new ArgumentException(string.Format(OpenGenericDecoratorRegistrationSourceResources.FromAndToMustDiffer, activatorData.FromService));
            }
            this._registrationData = registrationData;
            this._activatorData = activatorData;
        }

        private static IEnumerable<Parameter> AddDecoratedComponentParameter(Type decoratedParameterType, IComponentRegistration decoratedComponent, IEnumerable<Parameter> configuredParameters)
        {
            ResolvedParameter parameter = new ResolvedParameter((pi, c) => pi.ParameterType == decoratedParameterType, (pi, c) => c.ResolveComponent(decoratedComponent, Enumerable.Empty<Parameter>()));
            return new ResolvedParameter[] { parameter }.Concat<Parameter>(configuredParameters);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Type constructedImplementationType;
            IEnumerable<Service> services;
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (registrationAccessor == null)
            {
                throw new ArgumentNullException("registrationAccessor");
            }
            if (OpenGenericServiceBinder.TryBindServiceType(service, this._registrationData.Services, this._activatorData.ImplementationType, out constructedImplementationType, out services))
            {
                IServiceWithType swt = (IServiceWithType) service;
                Service arg = this._activatorData.FromService.ChangeType(swt.ServiceType);
                return (from cr in registrationAccessor(arg) select RegistrationBuilder.CreateRegistration(Guid.NewGuid(), this._registrationData, new ReflectionActivator(constructedImplementationType, this._activatorData.ConstructorFinder, this._activatorData.ConstructorSelector, AddDecoratedComponentParameter(swt.ServiceType, cr, this._activatorData.ConfiguredParameters), this._activatorData.ConfiguredProperties), services));
            }
            return Enumerable.Empty<IComponentRegistration>();
        }

        public override string ToString()
        {
            return string.Format(OpenGenericDecoratorRegistrationSourceResources.OpenGenericDecoratorRegistrationSourceImplFromTo, this._activatorData.ImplementationType.FullName, ((Service) this._activatorData.FromService).Description, string.Join(", ", (from s in this._registrationData.Services select s.Description).ToArray<string>()));
        }

        public bool IsAdapterForIndividualComponents
        {
            get
            {
                return true;
            }
        }
    }
}


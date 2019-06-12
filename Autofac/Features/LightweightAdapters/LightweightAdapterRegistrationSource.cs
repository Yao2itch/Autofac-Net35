namespace Autofac.Features.LightweightAdapters
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class LightweightAdapterRegistrationSource : IRegistrationSource
    {
        private readonly RegistrationData _registrationData;
        private readonly LightweightAdapterActivatorData _activatorData;

        public LightweightAdapterRegistrationSource(RegistrationData registrationData, LightweightAdapterActivatorData activatorData)
        {
            if (registrationData == null)
            {
                throw new ArgumentNullException("registrationData");
            }
            if (activatorData == null)
            {
                throw new ArgumentNullException("activatorData");
            }
            this._registrationData = registrationData;
            this._activatorData = activatorData;
            if (registrationData.Services.Contains<Service>(activatorData.FromService))
            {
                throw new ArgumentException(string.Format(LightweightAdapterRegistrationSourceResources.FromAndToMustDiffer, activatorData.FromService));
            }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            Func<IComponentRegistration, IComponentRegistration> selector = null;
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (registrationAccessor == null)
            {
                throw new ArgumentNullException("registrationAccessor");
            }
            if (!this._registrationData.Services.Contains<Service>(service))
            {
                return new IComponentRegistration[0];
            }
            if (selector == null)
            {
                selector = delegate (IComponentRegistration r) {
                    IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> rb = RegistrationBuilder.ForDelegate<object>((c, p) => this._activatorData.Adapter(c, p, c.ResolveComponent(r, Enumerable.Empty<Parameter>()))).Targeting<object, SimpleActivatorData, SingleRegistrationStyle>(r);
                    rb.RegistrationData.CopyFrom(this._registrationData, true);
                    return rb.CreateRegistration<object, SimpleActivatorData, SingleRegistrationStyle>();
                };
            }
            return registrationAccessor(this._activatorData.FromService).Select<IComponentRegistration, IComponentRegistration>(selector);
        }

        public override string ToString()
        {
            return string.Format(LightweightAdapterRegistrationSourceResources.AdapterFromToDescription, this._activatorData.FromService.Description, string.Join(", ", (from s in this._registrationData.Services select s.Description).ToArray<string>()));
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


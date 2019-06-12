namespace Autofac.Features.Metadata
{
    using Autofac;
    using Autofac.Builder;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class MetaRegistrationSource : IRegistrationSource
    {
        private static readonly MethodInfo CreateMetaRegistrationMethod = typeof(MetaRegistrationSource).GetMethod("CreateMetaRegistration", BindingFlags.NonPublic | BindingFlags.Static);

        private static IComponentRegistration CreateMetaRegistration<T>(Service providedService, IComponentRegistration valueRegistration)
        {
            return RegistrationBuilder.ForDelegate<Meta<T>>((c, p) => new Meta<T>((T) c.ResolveComponent(valueRegistration, p), valueRegistration.Target.Metadata)).As(new Service[] { providedService }).Targeting<Meta<T>, SimpleActivatorData, SingleRegistrationStyle>(valueRegistration).CreateRegistration<Meta<T>, SimpleActivatorData, SingleRegistrationStyle>();
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            IServiceWithType type = service as IServiceWithType;
            if ((type == null) || !type.ServiceType.IsGenericTypeDefinedBy(typeof(Meta<>)))
            {
                return Enumerable.Empty<IComponentRegistration>();
            }
            Type newType = type.ServiceType.GetGenericArguments()[0];
            Service arg = type.ChangeType(newType);
            MethodInfo registrationCreator = CreateMetaRegistrationMethod.MakeGenericMethod(new Type[] { newType });
            return (from v in registrationAccessor(arg) select registrationCreator.Invoke(null, new object[] { service, v })).Cast<IComponentRegistration>();
        }

        public override string ToString()
        {
            return MetaRegistrationSourceResources.MetaRegistrationSourceDescription;
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


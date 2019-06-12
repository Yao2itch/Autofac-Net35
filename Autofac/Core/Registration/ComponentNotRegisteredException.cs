namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using Util;
    using System;
    using System.Runtime.Serialization;
    using System.Globalization;

    [Serializable]
    public class ComponentNotRegisteredException : DependencyResolutionException
    {
        public ComponentNotRegisteredException(Service service) : base(string.Format(CultureInfo.CurrentCulture, ComponentNotRegisteredExceptionResources.Message, new object[] { Enforce.ArgumentNotNull<Service>(service, "service") }))
        {
        }

        public ComponentNotRegisteredException(Service service, Exception innerException) : base(string.Format(CultureInfo.CurrentCulture, ComponentNotRegisteredExceptionResources.Message, new object[] { Enforce.ArgumentNotNull<Service>(service, "service") }), innerException)
        {
        }

        protected ComponentNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}


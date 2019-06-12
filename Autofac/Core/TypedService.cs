namespace Autofac.Core
{
    using Autofac.Util;
    using System;
    using System.Runtime.CompilerServices;

    public sealed class TypedService : Service, IServiceWithType
    {
        public TypedService(Type serviceType)
        {
            this.ServiceType = Enforce.ArgumentNotNull<Type>(serviceType, "serviceType");
        }

        public Service ChangeType(Type newType)
        {
            if (newType == null)
            {
                throw new ArgumentNullException("newType");
            }
            return new TypedService(newType);
        }

        public override bool Equals(object obj)
        {
            TypedService service = obj as TypedService;
            if (service == null)
            {
                return false;
            }
            return this.ServiceType.IsCompatibleWith(service.ServiceType);
        }

        public override int GetHashCode()
        {
            return this.ServiceType.GetCompatibleHashCode();
        }

        public Type ServiceType { get; private set; }

        public override string Description
        {
            get
            {
                return this.ServiceType.FullName;
            }
        }
    }
}


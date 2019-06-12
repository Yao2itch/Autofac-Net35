namespace Autofac.Core
{
    using Autofac.Util;
    using System;

    public sealed class KeyedService : Service, IServiceWithType
    {
        private readonly object _serviceKey;
        private readonly Type _serviceType;

        public KeyedService(object serviceKey, Type serviceType)
        {
            this._serviceKey = Enforce.ArgumentNotNull<object>(serviceKey, "serviceKey");
            this._serviceType = Enforce.ArgumentNotNull<Type>(serviceType, "serviceType");
        }

        public Service ChangeType(Type newType)
        {
            if (newType == null)
            {
                throw new ArgumentNullException("newType");
            }
            return new KeyedService(this.ServiceKey, newType);
        }

        public override bool Equals(object obj)
        {
            KeyedService service = obj as KeyedService;
            if (service == null)
            {
                return false;
            }
            return (this.ServiceKey.Equals(service.ServiceKey) && this.ServiceType.IsCompatibleWith(service.ServiceType));
        }

        public override int GetHashCode()
        {
            return (this.ServiceKey.GetHashCode() ^ this.ServiceType.GetCompatibleHashCode());
        }

        public object ServiceKey
        {
            get
            {
                return this._serviceKey;
            }
        }

        public Type ServiceType
        {
            get
            {
                return this._serviceType;
            }
        }

        public override string Description
        {
            get
            {
                return string.Concat(new object[] { this.ServiceKey, " (", this.ServiceType.FullName, ")" });
            }
        }
    }
}


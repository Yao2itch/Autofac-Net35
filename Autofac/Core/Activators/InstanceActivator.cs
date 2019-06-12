namespace Autofac.Core.Activators
{
    using Autofac.Util;
    using System;

    public abstract class InstanceActivator : Disposable
    {
        private readonly Type _limitType;

        protected InstanceActivator(Type limitType)
        {
            this._limitType = Enforce.ArgumentNotNull<Type>(limitType, "limitType");
        }

        public override string ToString()
        {
            return (this.LimitType.Name + " (" + base.GetType().Name + ")");
        }

        public Type LimitType
        {
            get
            {
                return this._limitType;
            }
        }
    }
}


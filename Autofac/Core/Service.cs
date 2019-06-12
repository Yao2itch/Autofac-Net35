namespace Autofac.Core
{
    using System;

    public abstract class Service
    {
        protected Service()
        {
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException(ServiceResources.MustOverrideEquals);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException(ServiceResources.MustOverrideGetHashCode);
        }

        public static bool operator ==(Service lhs, Service rhs)
        {
            return object.Equals(lhs, rhs);
        }

        public static bool operator !=(Service lhs, Service rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return this.Description;
        }

        public abstract string Description { get; }
    }
}


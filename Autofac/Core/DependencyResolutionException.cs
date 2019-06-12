namespace Autofac.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DependencyResolutionException : Exception
    {
        public DependencyResolutionException(string message) : base(message)
        {
        }

        protected DependencyResolutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DependencyResolutionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}


namespace Autofac.Core.Resolving
{
    using System;

    public class InstanceLookupBeginningEventArgs : EventArgs
    {
        private readonly IInstanceLookup _instanceLookup;

        public InstanceLookupBeginningEventArgs(IInstanceLookup instanceLookup)
        {
            if (instanceLookup == null)
            {
                throw new ArgumentNullException("instanceLookup");
            }
            this._instanceLookup = instanceLookup;
        }

        public IInstanceLookup InstanceLookup
        {
            get
            {
                return this._instanceLookup;
            }
        }
    }
}


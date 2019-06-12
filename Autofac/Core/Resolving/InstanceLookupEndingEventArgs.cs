namespace Autofac.Core.Resolving
{
    using System;

    public class InstanceLookupEndingEventArgs : EventArgs
    {
        private readonly IInstanceLookup _instanceLookup;
        private readonly bool _newInstanceActivated;

        public InstanceLookupEndingEventArgs(IInstanceLookup instanceLookup, bool newInstanceActivated)
        {
            if (instanceLookup == null)
            {
                throw new ArgumentNullException("instanceLookup");
            }
            this._instanceLookup = instanceLookup;
            this._newInstanceActivated = newInstanceActivated;
        }

        public bool NewInstanceActivated
        {
            get
            {
                return this._newInstanceActivated;
            }
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


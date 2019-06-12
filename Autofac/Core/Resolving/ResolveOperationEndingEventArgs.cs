namespace Autofac.Core.Resolving
{
    using System;
    using System.Runtime.InteropServices;

    public class ResolveOperationEndingEventArgs : EventArgs
    {
        private readonly IResolveOperation _resolveOperation;
        private readonly System.Exception _exception;

        public ResolveOperationEndingEventArgs(IResolveOperation resolveOperation, System.Exception exception)
        {
            this._resolveOperation = resolveOperation;
            this._exception = exception;
        }

        public System.Exception Exception
        {
            get
            {
                return this._exception;
            }
        }

        public IResolveOperation ResolveOperation
        {
            get
            {
                return this._resolveOperation;
            }
        }
    }
}


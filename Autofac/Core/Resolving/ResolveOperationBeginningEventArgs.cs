namespace Autofac.Core.Resolving
{
    using System;

    public class ResolveOperationBeginningEventArgs : EventArgs
    {
        private readonly IResolveOperation _resolveOperation;

        public ResolveOperationBeginningEventArgs(IResolveOperation resolveOperation)
        {
            this._resolveOperation = resolveOperation;
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


namespace Autofac.Util
{
    using System;
    using System.Threading;

    public class Disposable : IDisposable
    {
        private const int DisposedFlag = 1;
        private int _isDisposed;

        public void Dispose()
        {
            int comparand = this._isDisposed;
            Interlocked.CompareExchange(ref this._isDisposed, 1, comparand);
            if (comparand == 0)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected bool IsDisposed
        {
            get
            {
                Thread.MemoryBarrier();
                return (this._isDisposed == 1);
            }
        }
    }
}


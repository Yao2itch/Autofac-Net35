namespace Autofac.Features.OwnedInstances
{
    using Autofac.Util;
    using System;
    using System.Threading;

    public class Owned<T> : Disposable
    {
        private T _value;
        private IDisposable _lifetime;

        public Owned(T value, IDisposable lifetime)
        {
            this._value = value;
            this._lifetime = Enforce.ArgumentNotNull<IDisposable>(lifetime, "lifetime");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable comparand = this._lifetime;
                Interlocked.CompareExchange<IDisposable>(ref this._lifetime, null, comparand);
                if (comparand != null)
                {
                    this._value = default(T);
                    comparand.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public T Value
        {
            get
            {
                return this._value;
            }
        }
    }
}


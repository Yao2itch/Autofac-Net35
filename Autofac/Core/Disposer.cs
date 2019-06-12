namespace Autofac.Core
{
    using Autofac.Util;
    using System;
    using System.Collections.Generic;

    internal class Disposer : Disposable, IDisposer, IDisposable
    {
        private Stack<IDisposable> _items = new Stack<IDisposable>();
        private readonly object _synchRoot = new object();

        public void AddInstanceForDisposal(IDisposable instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            lock (this._synchRoot)
            {
                this._items.Push(instance);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this._synchRoot)
                {
                    while (this._items.Count > 0)
                    {
                        this._items.Pop().Dispose();
                    }
                    this._items = null;
                }
            }
        }
    }
}


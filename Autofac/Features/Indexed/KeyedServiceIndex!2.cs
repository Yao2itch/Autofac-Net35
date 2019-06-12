namespace Autofac.Features.Indexed
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class KeyedServiceIndex<TKey, TValue> : IIndex<TKey, TValue>
    {
        private readonly IComponentContext _context;

        public KeyedServiceIndex(IComponentContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
        }

        private static KeyedService GetService(TKey key)
        {
            return new KeyedService(key, typeof(TValue));
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            object obj2;
            if (this._context.TryResolveService(KeyedServiceIndex<TKey, TValue>.GetService(key), out obj2))
            {
                value = (TValue) obj2;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                return (TValue) this._context.ResolveService(KeyedServiceIndex<TKey, TValue>.GetService(key));
            }
        }
    }
}


namespace Autofac.Features.Indexed
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public interface IIndex<TKey, TValue>
    {
        bool TryGetValue(TKey key, out TValue value);

        TValue this[TKey key] { get; }
    }
}


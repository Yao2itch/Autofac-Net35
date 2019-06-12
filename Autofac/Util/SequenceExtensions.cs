namespace Autofac.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal static class SequenceExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T local in items)
            {
                collection.Add(local);
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T trailingItem)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            foreach (var t in sequence)
                yield return t;

            yield return trailingItem;

            //return new <Append>d__0<T>(-2) { 
            //    <>3__sequence = sequence,
            //    <>3__trailingItem = trailingItem
            //};
        }

        public static string JoinWith(this IEnumerable<string> elements, string separator)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }
            if (separator == null)
            {
                throw new ArgumentNullException("separator");
            }
            return string.Join(separator, elements.ToArray<string>());
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T leadingItem)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            yield return leadingItem;

            foreach (var t in sequence)
                yield return t;

            //return new <Prepend>d__5<T>(-2) { 
            //    <>3__sequence = sequence,
            //    <>3__leadingItem = leadingItem
            //};
        }

        public static IEnumerable<V> Zip<T, U, V>(this IEnumerable<T> first, IEnumerable<U> second, Func<T, U, V> resultMapping)
        {
            if ( first == null ) throw new ArgumentNullException(nameof(first));
            if ( second == null ) throw new ArgumentNullException(nameof(second));
            
            foreach(var f in first)
            {
                foreach(var s in second)
                {
                    yield return resultMapping( f,s );
                }
            }

            //return new <Zip>d__a<T, U, V>(-2) { 
            //    <>3__first = first,
            //    <>3__second = second,
            //    <>3__resultMapping = resultMapping
            //};
        }

        
    }
}


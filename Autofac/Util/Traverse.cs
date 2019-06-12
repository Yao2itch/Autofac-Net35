namespace Autofac.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal static class Traverse
    {
        public static IEnumerable<T> Across<T>(T first, Func<T, T> next) where T: class
        {
            var item = first;
            while (item != null)
            {
                yield return item;
                item = next(item);
            }

            //return new <Across>d__0<T>(-2) { 
            //    <>3__first = first,
            //    <>3__next = next
            //};
        }
    }
}


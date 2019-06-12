namespace Autofac
{
    using Autofac.Util;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class TypeExtensions
    {
        public static bool IsAssignableTo<T>(this Type @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
            return typeof(T).IsAssignableFrom(@this);
        }

        public static bool IsClosedTypeOf(this Type @this, Type openGeneric)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
            if (openGeneric == null)
            {
                throw new ArgumentNullException("openGeneric");
            }
            if (!openGeneric.IsGenericTypeDefinition && !openGeneric.ContainsGenericParameters)
            {
                throw new ArgumentException(string.Format(TypeExtensionsResources.NotOpenGenericType, openGeneric.FullName));
            }
            return @this.GetTypesThatClose(openGeneric).Any<Type>();
        }

        public static bool IsInNamespace(this Type @this, string @namespace)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
            if (@namespace == null)
            {
                throw new ArgumentNullException("namespace");
            }
            if (@this.Namespace == null)
            {
                return false;
            }
            if (@this.Namespace != @namespace)
            {
                return @this.Namespace.StartsWith(@namespace + ".");
            }
            return true;
        }

        public static bool IsInNamespaceOf<T>(this Type @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
            return @this.IsInNamespace(typeof(T).Namespace);
        }
    }
}


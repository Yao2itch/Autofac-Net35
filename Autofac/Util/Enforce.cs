namespace Autofac.Util
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    internal static class Enforce
    {
        public static T ArgumentElementNotNull<T>(T value, string name) where T: class, IEnumerable
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            if (value.Cast<object>().Any<object>(v => v == null))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnforceResources.ElementCannotBeNull, new object[] { name }));
            }
            return value;
        }

        public static T ArgumentNotNull<T>(T value, string name) where T: class
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            return value;
        }

        public static string ArgumentNotNullOrEmpty(string value, string description)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            if (value == null)
            {
                throw new ArgumentNullException(description);
            }
            if (value == string.Empty)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnforceResources.CannotBeEmpty, new object[] { description }));
            }
            return value;
        }

        public static void ArgumentTypeIsFunction(Type delegateType)
        {
            if (delegateType == null)
            {
                throw new ArgumentNullException("delegateType");
            }
            MethodInfo method = delegateType.GetMethod("Invoke");
            if (method == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnforceResources.NotDelegate, new object[] { delegateType }));
            }
            if (method.ReturnType == typeof(void))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, EnforceResources.DelegateReturnsVoid, new object[] { delegateType }));
            }
        }

        public static T NotNull<T>(T value) where T: class
        {
            if (value == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, EnforceResources.CannotBeNull, new object[] { typeof(T).FullName }));
            }
            return value;
        }
    }
}


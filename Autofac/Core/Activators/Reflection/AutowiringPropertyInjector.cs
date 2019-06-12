namespace Autofac.Core.Activators.Reflection
{
    using Autofac;
    using System;
    using System.Linq;
    using System.Reflection;

    internal class AutowiringPropertyInjector
    {
        public void InjectProperties(IComponentContext context, object instance, bool overrideSetValues)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (instance == null) throw new ArgumentNullException("instance");

            var instanceType = instance.GetType();

            foreach (var property in instanceType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.CanWrite))
            {
                var propertyType = property.PropertyType;

                if (propertyType.IsValueType && !propertyType.IsEnum)
                    continue;

                if (propertyType.IsArray && propertyType.GetElementType().IsValueType)
                    continue;

                //if (propertyType.IsGenericEnumerableInterfaceType() && propertyType.GetGenericArguments()[0].IsValueType)
                //    continue;

                if (property.GetIndexParameters().Length != 0)
                    continue;

                if (!context.IsRegistered(propertyType))
                    continue;

                var accessors = property.GetAccessors(false);
                if (accessors.Length == 1 && accessors[0].ReturnType != typeof(void))
                    continue;

                if (!overrideSetValues &&
                    accessors.Length == 2 &&
                    (property.GetValue(instance, null) != null))
                    continue;

                var propertyValue = context.Resolve(propertyType);
                property.SetValue(instance, propertyValue, null);
            }

            //if (context == null)
            //{
            //    throw new ArgumentNullException("context");
            //}
            //if (instance == null)
            //{
            //    throw new ArgumentNullException("instance");
            //}
            //foreach (PropertyInfo info in from pi in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            //    where pi.CanWrite
            //    select pi)
            //{
            //    Type propertyType = info.PropertyType;
            //    if ((!propertyType.IsValueType || propertyType.IsEnum) && ((info.GetIndexParameters().Length == 0) && context.IsRegistered(propertyType)))
            //    {
            //        MethodInfo[] accessors = info.GetAccessors(false);
            //        if (((accessors.Length != 1) || (accessors[0].ReturnType == typeof(void))) && ((overrideSetValues || (accessors.Length != 2)) || (info.GetValue(instance, null) == null)))
            //        {
            //            object obj2 = context.Resolve(propertyType);
            //            info.SetValue(instance, obj2, null);
            //        }
            //    }
            //}
        }
    }
}


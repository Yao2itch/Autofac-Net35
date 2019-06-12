namespace Autofac.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class TypeExtensions
    {
        private static IEnumerable<Type> FindAssignableTypesThatClose(Type candidateType, Type openGenericServiceType)
        {
            return (from t in TypesAssignableFrom(candidateType)
                where t.IsGenericTypeDefinedBy(openGenericServiceType)
                select t);
        }

        public static Type FunctionReturnType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            MethodInfo method = type.GetMethod("Invoke");
            Enforce.NotNull<MethodInfo>(method);
            return method.ReturnType;
        }

        public static int GetCompatibleHashCode(this Type type)
        {
            if (type.IsImport)
            {
                return type.GUID.GetHashCode();
            }
            return type.GetHashCode();
        }

        public static IEnumerable<Type> GetTypesThatClose(this Type @this, Type openGeneric)
        {
            return FindAssignableTypesThatClose(@this, openGeneric);
        }

        public static bool IsCompatibleWith(this Type type, Type that)
        {
            return type.Equals(that);
        }

        public static bool IsCompatibleWithGenericParameterConstraints(this Type genericTypeDefinition, Type[] parameters)
        {
            Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                Type type = genericArguments[i];
                Type parameter = parameters[i];
                if (type.GetGenericParameterConstraints().Any<Type>(constraint => !ParameterCompatibleWithTypeConstraint(parameter, constraint)))
                {
                    return false;
                }
                GenericParameterAttributes genericParameterAttributes = type.GenericParameterAttributes;
                if ((((genericParameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) != GenericParameterAttributes.None) && !parameter.IsValueType) && (parameter.GetConstructor(Type.EmptyTypes) == null))
                {
                    return false;
                }
                if (((genericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != GenericParameterAttributes.None) && parameter.IsValueType)
                {
                    return false;
                }
                if (((genericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None) && (!parameter.IsValueType || (parameter.IsGenericType && parameter.IsGenericTypeDefinedBy(typeof(Nullable<>)))))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsDelegate(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return type.IsSubclassOf(typeof(Delegate));
        }

        public static bool IsGenericTypeDefinedBy(this Type @this, Type openGeneric)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }
            if (openGeneric == null)
            {
                throw new ArgumentNullException("openGeneric");
            }
            return ((!@this.ContainsGenericParameters && @this.IsGenericType) && (@this.GetGenericTypeDefinition() == openGeneric));
        }

        private static bool ParameterCompatibleWithTypeConstraint(Type parameter, Type constraint)
        {
            return (constraint.IsAssignableFrom(parameter) || Traverse.Across<Type>(parameter, p => p.BaseType).Concat<Type>(parameter.GetInterfaces()).Any<Type>(p => p.GUID.Equals(constraint.GUID)));
        }

        private static IEnumerable<Type> TypesAssignableFrom(Type candidateType)
        {
            return candidateType.GetInterfaces().Concat<Type>(Traverse.Across<Type>(candidateType, t => t.BaseType));
        }
    }
}


namespace Autofac.Util
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class ReflectionExtensions
    {
        public static MethodInfo GetMethod<TDeclaring>(Expression<Action<TDeclaring>> methodCallExpression)
        {
            if (methodCallExpression == null)
            {
                throw new ArgumentNullException("methodCallExpression");
            }
            MethodCallExpression body = methodCallExpression.Body as MethodCallExpression;
            if (body == null)
            {
                throw new ArgumentException(string.Format(ReflectionExtensionsResources.ExpressionNotMethodCall, methodCallExpression));
            }
            return body.Method;
        }

        public static PropertyInfo GetProperty<TDeclaring, TProperty>(Expression<Func<TDeclaring, TProperty>> propertyAccessor)
        {
            if (propertyAccessor == null)
            {
                throw new ArgumentNullException("propertyAccessor");
            }
            MemberExpression body = propertyAccessor.Body as MemberExpression;
            if ((body == null) || !(body.Member is PropertyInfo))
            {
                throw new ArgumentException(string.Format(ReflectionExtensionsResources.ExpressionNotPropertyAccessor, propertyAccessor));
            }
            return (PropertyInfo) body.Member;
        }

        public static bool TryGetDeclaringProperty(this ParameterInfo pi, out PropertyInfo prop)
        {
            MethodInfo member = pi.Member as MethodInfo;
            if (((member != null) && member.IsSpecialName) && member.Name.StartsWith("set_"))
            {
                prop = member.DeclaringType.GetProperty(member.Name.Substring(4));
                return true;
            }
            prop = null;
            return false;
        }
    }
}


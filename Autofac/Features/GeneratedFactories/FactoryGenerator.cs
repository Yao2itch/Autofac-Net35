namespace Autofac.Features.GeneratedFactories
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class FactoryGenerator
    {
        private readonly Func<IComponentContext, IEnumerable<Parameter>, Delegate> _generator;

        public FactoryGenerator(Type delegateType, IComponentRegistration productRegistration, ParameterMapping parameterMapping)
        {
            Func<Expression, Expression[], Expression> makeResolveCall = null;
            if (productRegistration == null)
            {
                throw new ArgumentNullException("productRegistration");
            }
            Enforce.ArgumentTypeIsFunction(delegateType);
            if (makeResolveCall == null)
            {
                makeResolveCall = (activatorContextParam, resolveParameterArray) => Expression.Call(activatorContextParam, ReflectionExtensions.GetMethod<IComponentContext>((Expression<Action<IComponentContext>>) (cc => cc.ResolveComponent(null, null))), new Expression[] { Expression.Constant(productRegistration, typeof(IComponentRegistration)), Expression.NewArrayInit(typeof(Parameter), resolveParameterArray) });
            }
            this._generator = CreateGenerator(makeResolveCall, delegateType, GetParameterMapping(delegateType, parameterMapping));
        }

        public FactoryGenerator(Type delegateType, Service service, ParameterMapping parameterMapping)
        {
            Func<Expression, Expression[], Expression> makeResolveCall = null;
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            Enforce.ArgumentTypeIsFunction(delegateType);
            if (makeResolveCall == null)
            {
                makeResolveCall = (activatorContextParam, resolveParameterArray) => Expression.Call(ReflectionExtensions.GetMethod<IComponentContext>((Expression<Action<IComponentContext>>) (cc => cc.ResolveService(null, ((Parameter[]) null)))), new Expression[] { activatorContextParam, Expression.Constant(service, typeof(Service)), Expression.NewArrayInit(typeof(Parameter), resolveParameterArray) });
            }
            this._generator = CreateGenerator(makeResolveCall, delegateType, GetParameterMapping(delegateType, parameterMapping));
        }

        private static Func<IComponentContext, IEnumerable<Parameter>, Delegate> CreateGenerator(Func<Expression, Expression[], Expression> makeResolveCall, Type delegateType, ParameterMapping pm)
        {
            ParameterExpression expression;
            ParameterExpression expression2;
            ParameterExpression[] parameters = new ParameterExpression[] { expression = Expression.Parameter(typeof(IComponentContext), "c"), expression2 = Expression.Parameter(typeof(IEnumerable<Parameter>), "p") };
            MethodInfo method = delegateType.GetMethod("Invoke");
            List<ParameterExpression> creatorParams = (from pi in method.GetParameters() select Expression.Parameter(pi.ParameterType, pi.Name)).ToList<ParameterExpression>();
            Expression[] expressionArray2 = MapParameters(creatorParams, pm);
            UnaryExpression body = Expression.Convert(makeResolveCall(expression, expressionArray2), method.ReturnType);
            return Expression.Lambda<Func<IComponentContext, IEnumerable<Parameter>, Delegate>>(Expression.Lambda(delegateType, body, creatorParams), parameters).Compile();
        }

        public Delegate GenerateFactory(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            return this._generator(context.Resolve<ILifetimeScope>(), parameters);
        }

        public TDelegate GenerateFactory<TDelegate>(IComponentContext context, IEnumerable<Parameter> parameters) where TDelegate: class
        {
            return (TDelegate)(object) this.GenerateFactory(context, parameters);
        }

        private static ParameterMapping GetParameterMapping(Type delegateType, ParameterMapping configuredParameterMapping)
        {
            if (configuredParameterMapping != ParameterMapping.Adaptive)
            {
                return configuredParameterMapping;
            }
            if (!delegateType.Name.StartsWith("Func`"))
            {
                return ParameterMapping.ByName;
            }
            return ParameterMapping.ByType;
        }

        private static Expression[] MapParameters(IEnumerable<ParameterExpression> creatorParams, ParameterMapping pm)
        {
            switch (pm)
            {
                case ParameterMapping.ByType:
                    return (from p in creatorParams select Expression.New(typeof(TypedParameter).GetConstructor(new Type[] { typeof(Type), typeof(object) }), new Expression[] { Expression.Constant(p.Type, typeof(Type)), Expression.Convert(p, typeof(object)) })).OfType<Expression>().ToArray<Expression>();

                case ParameterMapping.ByPosition:
                    return creatorParams.Select<ParameterExpression, NewExpression>((p, i) => Expression.New(typeof(PositionalParameter).GetConstructor(new Type[] { typeof(int), typeof(object) }), new Expression[] { Expression.Constant(i, typeof(int)), Expression.Convert(p, typeof(object)) })).OfType<Expression>().ToArray<Expression>();
            }
            return (from p in creatorParams select Expression.New(typeof(NamedParameter).GetConstructor(new Type[] { typeof(string), typeof(object) }), new Expression[] { Expression.Constant(p.Name, typeof(string)), Expression.Convert(p, typeof(object)) })).OfType<Expression>().ToArray<Expression>();
        }
    }
}


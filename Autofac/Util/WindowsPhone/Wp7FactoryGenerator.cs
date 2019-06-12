namespace Autofac.Util.WindowsPhone
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Features.GeneratedFactories;
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Wp7FactoryGenerator
    {
        private static readonly MethodInfo[] DelegateActivators = (from x in typeof(Wp7FactoryGenerator).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            where x.Name == "DelegateActivator"
            select x).ToArray<MethodInfo>();
        private Func<IComponentContext, IEnumerable<Parameter>, Delegate> _generator;

        public Wp7FactoryGenerator(Type delegateType, IComponentRegistration service, ParameterMapping parameterMapping)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            Enforce.ArgumentTypeIsFunction(delegateType);
            this.CreateGenerator(service, delegateType, parameterMapping);
        }

        public Wp7FactoryGenerator(Type delegateType, Service service, ParameterMapping parameterMapping)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            Enforce.ArgumentTypeIsFunction(delegateType);
            this.CreateGenerator(service, delegateType, parameterMapping);
        }
        
        private void CreateGenerator(object service, Type delegateType, ParameterMapping parameterMapping)
        {
            Type trailingItem = delegateType.FunctionReturnType();
            MethodInfo method = delegateType.GetMethod("Invoke");
            List<Type> args = (from x in method.GetParameters() select x.ParameterType).Append<Type>(trailingItem).Append<Type>(delegateType).ToList<Type>();
            MethodInfo info2 = DelegateActivators.FirstOrDefault<MethodInfo>(x => x.GetGenericArguments().Count<Type>() == args.Count<Type>());
            if (info2 != null)
            {
                MethodInfo creator = info2.MakeGenericMethod(args.ToArray());
                this._generator = (a0, a1) => (Delegate) creator.Invoke(null, new object[] { a0, service, parameterMapping.ResolveParameterMapping(delegateType) });
            }
        }

        private static Delegate DelegateActivator<TResult, TDelegate>(IComponentContext context, object target, ParameterMapping mapping)
        {
            ILifetimeScope ls = context.Resolve<ILifetimeScope>();
            Func<TResult> func = () => Resolve<TResult>(target, ls, new List<Parameter>());
            return Delegate.CreateDelegate(typeof(TDelegate), func.Target, func.Method);
        }

        private static Delegate DelegateActivator<TArg0, TResult, TDelegate>(IComponentContext context, object target, ParameterMapping mapping)
        {
            ILifetimeScope ls = context.Resolve<ILifetimeScope>();
            Func<TArg0, TResult> func = a0 => Resolve<TResult>(target, ls, mapping.GetParameterCollection<TDelegate>(new object[] { a0 }));
            return Delegate.CreateDelegate(typeof(TDelegate), func.Target, func.Method);
        }

        private static Delegate DelegateActivator<TArg0, TArg1, TResult, TDelegate>(IComponentContext context, object target, ParameterMapping mapping)
        {
            ILifetimeScope ls = context.Resolve<ILifetimeScope>();
            Func<TArg0, TArg1, TResult> func = (a0, a1) => Resolve<TResult>(target, ls, mapping.GetParameterCollection<TDelegate>(new object[] { a0, a1 }));
            return Delegate.CreateDelegate(typeof(TDelegate), func.Target, func.Method);
        }

        private static Delegate DelegateActivator<TArg0, TArg1, TArg2, TResult, TDelegate>(IComponentContext context, object target, ParameterMapping mapping)
        {
            ILifetimeScope ls = context.Resolve<ILifetimeScope>();
            Func<TArg0, TArg1, TArg2, TResult> func = (a0, a1, a2) => Resolve<TResult>(target, ls, mapping.GetParameterCollection<TDelegate>(new object[] { a0, a1, a2 }));
            return Delegate.CreateDelegate(typeof(TDelegate), func.Target, func.Method);
        }

        private static Delegate DelegateActivator<TArg0, TArg1, TArg2, TArg3, TResult, TDelegate>(IComponentContext context, object target, ParameterMapping mapping)
        {
            ILifetimeScope ls = context.Resolve<ILifetimeScope>();
            Func<TArg0, TArg1, TArg2, TArg3, TResult> func = (a0, a1, a2, a3) => Resolve<TResult>(target, ls, mapping.GetParameterCollection<TDelegate>(new object[] { a0, a1, a2, a3 }));
            return Delegate.CreateDelegate(typeof(TDelegate), func.Target, func.Method);
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
            return (TDelegate) (object)this.GenerateFactory(context, parameters);
        }

        private static TResult Resolve<TResult>(object target, IComponentContext ls, IEnumerable<Parameter> parameterCollection)
        {
            object obj2;
            if (target is Service)
            {
                obj2 = ls.ResolveService((Service) target, parameterCollection);
            }
            else
            {
                obj2 = ls.ResolveComponent((IComponentRegistration) target, parameterCollection);
            }
            return (TResult) obj2;
        }
    }
}


namespace Autofac.Core.Activators.Reflection
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MostParametersConstructorSelector : IConstructorSelector
    {
        public ConstructorParameterBinding SelectConstructorBinding(ConstructorParameterBinding[] constructorBindings)
        {
            if (constructorBindings == null)
            {
                throw new ArgumentNullException("constructorBindings");
            }
            if (constructorBindings.Length == 0)
            {
                throw new ArgumentOutOfRangeException("constructorBindings");
            }
            if (constructorBindings.Length == 1)
            {
                return constructorBindings[0];
            }
            var source = from binding in constructorBindings select new { 
                Binding = binding,
                ConstructorParameterLength = binding.TargetConstructor.GetParameters().Length
            };
            int maxLength = source.Max(binding => binding.ConstructorParameterLength);
            ConstructorParameterBinding[] bindingArray = (from ctor in source
                where ctor.ConstructorParameterLength == maxLength
                select ctor.Binding).ToArray<ConstructorParameterBinding>();
            if (bindingArray.Length != 1)
            {
                throw new DependencyResolutionException(string.Format("Cannot choose between multiple constructors with equal length {0} on type '{1}'. Select the constructor explicitly, with the UsingConstructor() configuration method, when the component is registered.", maxLength, bindingArray[0].TargetConstructor.DeclaringType));
            }
            return bindingArray[0];
        }
    }
}


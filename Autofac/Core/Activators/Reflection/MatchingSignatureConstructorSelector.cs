namespace Autofac.Core.Activators.Reflection
{
    using Autofac.Core;
    using System;
    using System.Linq;
    using System.Reflection;

    public class MatchingSignatureConstructorSelector : IConstructorSelector
    {
        private readonly Type[] _signature;

        public MatchingSignatureConstructorSelector(params Type[] signature)
        {
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            this._signature = signature;
        }

        public ConstructorParameterBinding SelectConstructorBinding(ConstructorParameterBinding[] constructorBindings)
        {
            if (constructorBindings == null)
            {
                throw new ArgumentNullException("constructorBindings");
            }
            ConstructorParameterBinding[] bindingArray = (from b in constructorBindings
                where (from p in b.TargetConstructor.GetParameters() select p.ParameterType).SequenceEqual<Type>(this._signature)
                select b).ToArray<ConstructorParameterBinding>();
            if (bindingArray.Length == 1)
            {
                return bindingArray[0];
            }
            if (!constructorBindings.Any<ConstructorParameterBinding>())
            {
                throw new ArgumentException(MatchingSignatureConstructorSelectorResources.AtLeastOneBindingRequired);
            }
            string name = constructorBindings.First<ConstructorParameterBinding>().TargetConstructor.DeclaringType.Name;
            string str2 = string.Join(", ", (from t in this._signature select t.Name).ToArray<string>());
            if (bindingArray.Length == 0)
            {
                throw new DependencyResolutionException(string.Format(MatchingSignatureConstructorSelectorResources.RequiredConstructorNotAvailable, name, str2));
            }
            throw new DependencyResolutionException(string.Format(MatchingSignatureConstructorSelectorResources.TooManyConstructorsMatch, str2));
        }
    }
}


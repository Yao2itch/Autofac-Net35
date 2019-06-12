namespace Autofac.Core.Activators.Reflection
{
    using System;
    using System.Reflection;

    public class BindingFlagsConstructorFinder : IConstructorFinder
    {
        private readonly BindingFlags _bindingFlags;

        public BindingFlagsConstructorFinder(BindingFlags bindingFlags)
        {
            this._bindingFlags = bindingFlags;
        }

        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            return targetType.GetConstructors(BindingFlags.Instance | this._bindingFlags);
        }

        public override string ToString()
        {
            return string.Format(BindingFlagsConstructorFinderResources.HasBindingFlags, this._bindingFlags);
        }
    }
}


namespace Autofac.Core.Lifetime
{
    using Autofac.Core;
    using System;

    public class CurrentScopeLifetime : IComponentLifetime
    {
        public ISharingLifetimeScope FindScope(ISharingLifetimeScope mostNestedVisibleScope)
        {
            if (mostNestedVisibleScope == null)
            {
                throw new ArgumentNullException("mostNestedVisibleScope");
            }
            return mostNestedVisibleScope;
        }
    }
}


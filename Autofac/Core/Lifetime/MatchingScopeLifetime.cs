namespace Autofac.Core.Lifetime
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public class MatchingScopeLifetime : IComponentLifetime
    {
        private readonly Func<ILifetimeScope, bool> _matcher;
        private readonly string _matchExpressionCode;

        public MatchingScopeLifetime(Expression<Func<ILifetimeScope, bool>> matchExpression)
        {
            if (matchExpression == null)
            {
                throw new ArgumentNullException("matchExpression");
            }
            this._matcher = matchExpression.Compile();
            this._matchExpressionCode = matchExpression.Body.ToString();
        }

        public MatchingScopeLifetime(object lifetimeScopeTagToMatch)
        {
            Func<ILifetimeScope, bool> func = null;
            if (lifetimeScopeTagToMatch == null)
            {
                throw new ArgumentNullException("lifetimeScopeTagToMatch");
            }
            if (func == null)
            {
                func = ls => lifetimeScopeTagToMatch.Equals(ls.Tag);
            }
            this._matcher = func;
            this._matchExpressionCode = lifetimeScopeTagToMatch.ToString();
        }

        public ISharingLifetimeScope FindScope(ISharingLifetimeScope mostNestedVisibleScope)
        {
            if (mostNestedVisibleScope == null)
            {
                throw new ArgumentNullException("mostNestedVisibleScope");
            }
            for (ISharingLifetimeScope scope = mostNestedVisibleScope; scope != null; scope = scope.ParentLifetimeScope)
            {
                if (this._matcher(scope))
                {
                    return scope;
                }
            }
            throw new DependencyResolutionException(string.Format(CultureInfo.CurrentCulture, MatchingScopeLifetimeResources.MatchingScopeNotFound, new object[] { this._matchExpressionCode }));
        }
    }
}


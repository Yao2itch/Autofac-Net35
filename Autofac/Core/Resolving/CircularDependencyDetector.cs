﻿namespace Autofac.Core.Resolving
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class CircularDependencyDetector
    {
        private const int MaxResolveDepth = 50;

        public void CheckForCircularDependency(IComponentRegistration registration, Stack<InstanceLookup> activationStack, int callDepth)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (activationStack == null)
            {
                throw new ArgumentNullException("activationStack");
            }
            if (callDepth > 50)
            {
                throw new DependencyResolutionException(string.Format(CultureInfo.CurrentCulture, CircularDependencyDetectorResources.MaxDepthExceeded, new object[] { registration }));
            }
            if (IsCircularDependency(registration, activationStack))
            {
                throw new DependencyResolutionException(string.Format(CultureInfo.CurrentCulture, CircularDependencyDetectorResources.CircularDependency, new object[] { CreateDependencyGraphTo(registration, activationStack) }));
            }
        }

        private static string CreateDependencyGraphTo(IComponentRegistration registration, IEnumerable<InstanceLookup> activationStack)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (activationStack == null)
            {
                throw new ArgumentNullException("activationStack");
            }
            string str = Display(registration);
            foreach (IComponentRegistration registration2 in from a in activationStack select a.ComponentRegistration)
            {
                str = Display(registration2) + " -> " + str;
            }
            return str;
        }

        private static string Display(IComponentRegistration registration)
        {
            return (registration.Activator.LimitType.FullName ?? string.Empty);
        }

        private static bool IsCircularDependency(IComponentRegistration registration, IEnumerable<InstanceLookup> activationStack)
        {
            return activationStack.Any<InstanceLookup>(a => (a.ComponentRegistration == registration));
        }
    }
}


namespace Autofac.Core.Activators.Reflection
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class MatchingSignatureConstructorSelectorResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal MatchingSignatureConstructorSelectorResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Core.Activators.Reflection.MatchingSignatureConstructorSelectorResources", typeof(MatchingSignatureConstructorSelectorResources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string AtLeastOneBindingRequired
        {
            get
            {
                return ResourceManager.GetString("AtLeastOneBindingRequired", resourceCulture);
            }
        }

        internal static string RequiredConstructorNotAvailable
        {
            get
            {
                return ResourceManager.GetString("RequiredConstructorNotAvailable", resourceCulture);
            }
        }

        internal static string TooManyConstructorsMatch
        {
            get
            {
                return ResourceManager.GetString("TooManyConstructorsMatch", resourceCulture);
            }
        }
    }
}


namespace Autofac.Features.LazyDependencies
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated, DebuggerNonUserCode]
    internal class LazyRegistrationSourceResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal LazyRegistrationSourceResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Features.LazyDependencies.LazyRegistrationSourceResources", typeof(LazyRegistrationSourceResources).Assembly);
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

        internal static string LazyRegistrationSourceDescription
        {
            get
            {
                return ResourceManager.GetString("LazyRegistrationSourceDescription", resourceCulture);
            }
        }

        internal static string LazyWithMetadataRegistrationSourceDescription
        {
            get
            {
                return ResourceManager.GetString("LazyWithMetadataRegistrationSourceDescription", resourceCulture);
            }
        }
    }
}


namespace Autofac.Features.ResolveAnything
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class AnyConcreteTypeNotAlreadyRegisteredSourceResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal AnyConcreteTypeNotAlreadyRegisteredSourceResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Features.ResolveAnything.AnyConcreteTypeNotAlreadyRegisteredSourceResources", typeof(AnyConcreteTypeNotAlreadyRegisteredSourceResources).Assembly);
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

        internal static string AnyConcreteTypeNotAlreadyRegisteredSourceDescription
        {
            get
            {
                return ResourceManager.GetString("AnyConcreteTypeNotAlreadyRegisteredSourceDescription", resourceCulture);
            }
        }
    }
}


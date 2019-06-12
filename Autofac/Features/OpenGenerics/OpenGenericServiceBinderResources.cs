namespace Autofac.Features.OpenGenerics
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated, DebuggerNonUserCode]
    internal class OpenGenericServiceBinderResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal OpenGenericServiceBinderResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Features.OpenGenerics.OpenGenericServiceBinderResources", typeof(OpenGenericServiceBinderResources).Assembly);
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

        internal static string ImplementorMustBeOpenGenericTypeDefinition
        {
            get
            {
                return ResourceManager.GetString("ImplementorMustBeOpenGenericTypeDefinition", resourceCulture);
            }
        }

        internal static string InterfaceIsNotImplemented
        {
            get
            {
                return ResourceManager.GetString("InterfaceIsNotImplemented", resourceCulture);
            }
        }

        internal static string ServiceTypeMustBeOpenGenericTypeDefinition
        {
            get
            {
                return ResourceManager.GetString("ServiceTypeMustBeOpenGenericTypeDefinition", resourceCulture);
            }
        }

        internal static string TypesAreNotConvertible
        {
            get
            {
                return ResourceManager.GetString("TypesAreNotConvertible", resourceCulture);
            }
        }
    }
}


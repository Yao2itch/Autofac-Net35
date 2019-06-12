namespace Autofac.Core.Activators.Reflection
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class ConstructorParameterBindingResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal ConstructorParameterBindingResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Core.Activators.Reflection.ConstructorParameterBindingResources", typeof(ConstructorParameterBindingResources).Assembly);
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

        internal static string BoundConstructor
        {
            get
            {
                return ResourceManager.GetString("BoundConstructor", resourceCulture);
            }
        }

        internal static string CannotInstantitate
        {
            get
            {
                return ResourceManager.GetString("CannotInstantitate", resourceCulture);
            }
        }

        internal static string ExceptionDuringInstantiation
        {
            get
            {
                return ResourceManager.GetString("ExceptionDuringInstantiation", resourceCulture);
            }
        }

        internal static string NonBindableConstructor
        {
            get
            {
                return ResourceManager.GetString("NonBindableConstructor", resourceCulture);
            }
        }
    }
}


namespace Autofac.Features.OpenGenerics
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class OpenGenericDecoratorRegistrationSourceResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal OpenGenericDecoratorRegistrationSourceResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Features.OpenGenerics.OpenGenericDecoratorRegistrationSourceResources", typeof(OpenGenericDecoratorRegistrationSourceResources).Assembly);
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

        internal static string FromAndToMustDiffer
        {
            get
            {
                return ResourceManager.GetString("FromAndToMustDiffer", resourceCulture);
            }
        }

        internal static string OpenGenericDecoratorRegistrationSourceImplFromTo
        {
            get
            {
                return ResourceManager.GetString("OpenGenericDecoratorRegistrationSourceImplFromTo", resourceCulture);
            }
        }
    }
}


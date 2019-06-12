namespace Autofac.Core.Resolving
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class ResolveOperationResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal ResolveOperationResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Core.Resolving.ResolveOperationResources", typeof(ResolveOperationResources).Assembly);
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

        internal static string ExceptionDuringResolve
        {
            get
            {
                return ResourceManager.GetString("ExceptionDuringResolve", resourceCulture);
            }
        }

        internal static string MaxDepthExceeded
        {
            get
            {
                return ResourceManager.GetString("MaxDepthExceeded", resourceCulture);
            }
        }

        internal static string TemporaryContextDisposed
        {
            get
            {
                return ResourceManager.GetString("TemporaryContextDisposed", resourceCulture);
            }
        }
    }
}


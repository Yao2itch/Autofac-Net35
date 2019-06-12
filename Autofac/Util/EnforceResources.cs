namespace Autofac.Util
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class EnforceResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal EnforceResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Util.EnforceResources", typeof(EnforceResources).Assembly);
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

        internal static string CannotBeEmpty
        {
            get
            {
                return ResourceManager.GetString("CannotBeEmpty", resourceCulture);
            }
        }

        internal static string CannotBeNull
        {
            get
            {
                return ResourceManager.GetString("CannotBeNull", resourceCulture);
            }
        }

        internal static string DelegateReturnsVoid
        {
            get
            {
                return ResourceManager.GetString("DelegateReturnsVoid", resourceCulture);
            }
        }

        internal static string ElementCannotBeNull
        {
            get
            {
                return ResourceManager.GetString("ElementCannotBeNull", resourceCulture);
            }
        }

        internal static string NotDelegate
        {
            get
            {
                return ResourceManager.GetString("NotDelegate", resourceCulture);
            }
        }
    }
}


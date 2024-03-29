﻿namespace Autofac.Core.Activators.Reflection
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class ReflectionActivatorResources
    {
        private static System.Resources.ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal ReflectionActivatorResources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Autofac.Core.Activators.Reflection.ReflectionActivatorResources", typeof(ReflectionActivatorResources).Assembly);
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

        internal static string NoConstructorsAvailable
        {
            get
            {
                return ResourceManager.GetString("NoConstructorsAvailable", resourceCulture);
            }
        }

        internal static string NoConstructorsBindable
        {
            get
            {
                return ResourceManager.GetString("NoConstructorsBindable", resourceCulture);
            }
        }
    }
}


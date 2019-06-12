namespace Autofac
{
    using Autofac.Core;
    using System;
    using System.Reflection;

    public abstract class Module : IModule
    {
        protected Module()
        {
        }

        protected virtual void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
        }

        private void AttachToRegistrations(IComponentRegistry componentRegistry)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            foreach (IComponentRegistration registration in componentRegistry.Registrations)
            {
                this.AttachToComponentRegistration(componentRegistry, registration);
            }
            componentRegistry.Registered += (sender, e) => this.AttachToComponentRegistration(e.ComponentRegistry, e.ComponentRegistration);
        }

        protected virtual void AttachToRegistrationSource(IComponentRegistry componentRegistry, IRegistrationSource registrationSource)
        {
        }

        private void AttachToSources(IComponentRegistry componentRegistry)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            foreach (IRegistrationSource source in componentRegistry.Sources)
            {
                this.AttachToRegistrationSource(componentRegistry, source);
            }
            componentRegistry.RegistrationSourceAdded += (sender, e) => this.AttachToRegistrationSource(e.ComponentRegistry, e.RegistrationSource);
        }

        public void Configure(IComponentRegistry componentRegistry)
        {
            if (componentRegistry == null)
            {
                throw new ArgumentNullException("componentRegistry");
            }
            ContainerBuilder builder = new ContainerBuilder();
            this.Load(builder);
            builder.Update(componentRegistry);
            this.AttachToRegistrations(componentRegistry);
            this.AttachToSources(componentRegistry);
        }

        protected virtual void Load(ContainerBuilder builder)
        {
        }

        protected virtual Assembly ThisAssembly
        {
            get
            {
                Type type = base.GetType();
                if (type.BaseType != typeof(Autofac.Module))
                {
                    throw new InvalidOperationException("Module.ThisAssembly is only available in modules that inherit directly from Module.");
                }
                return type.Assembly;
            }
        }
    }
}


namespace Autofac.Builder
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Reflection;
    using Autofac.Core.Lifetime;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    internal class RegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> : IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>, IHideObjectMembers
    {
        private readonly TActivatorData _activatorData;
        private readonly TRegistrationStyle _registrationStyle;
        private readonly Autofac.Builder.RegistrationData _registrationData;

        public RegistrationBuilder(Service defaultService, TActivatorData activatorData, TRegistrationStyle style)
        {
            if (defaultService == null)
            {
                throw new ArgumentNullException("defaultService");
            }
            if (activatorData == null)
            {
                throw new ArgumentNullException("activatorData");
            }
            if (style == null)
            {
                throw new ArgumentNullException("style");
            }
            this._activatorData = activatorData;
            this._registrationStyle = style;
            this._registrationData = new Autofac.Builder.RegistrationData(defaultService);
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> As<TService>()
        {
            return this.As(new Service[] { new TypedService(typeof(TService)) });
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> As<TService1, TService2>()
        {
            return this.As(new Service[] { new TypedService(typeof(TService1)), new TypedService(typeof(TService2)) });
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> As<TService1, TService2, TService3>()
        {
            return this.As(new Service[] { new TypedService(typeof(TService1)), new TypedService(typeof(TService2)), new TypedService(typeof(TService3)) });
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> As(params Service[] services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            this.RegistrationData.AddServices(services);
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> As(params Type[] services)
        {
            return this.As(services.Select<Type, TypedService>(delegate (Type t) {
                if (t.FullName == null)
                {
                    return new TypedService(t.GetGenericTypeDefinition());
                }
                return new TypedService(t);
            }).Cast<Service>().ToArray<Service>());
        }

        Type IHideObjectMembers.GetType()
        {
            return base.GetType();
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ExternallyOwned()
        {
            this.RegistrationData.Ownership = InstanceOwnership.ExternallyOwned;
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerDependency()
        {
            this.RegistrationData.Sharing = InstanceSharing.None;
            this.RegistrationData.Lifetime = new CurrentScopeLifetime();
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerLifetimeScope()
        {
            this.RegistrationData.Sharing = InstanceSharing.Shared;
            this.RegistrationData.Lifetime = new CurrentScopeLifetime();
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerMatchingLifetimeScope(object lifetimeScopeTag)
        {
            if (lifetimeScopeTag == null)
            {
                throw new ArgumentNullException("lifetimeScopeTag");
            }
            this.RegistrationData.Sharing = InstanceSharing.Shared;
            this.RegistrationData.Lifetime = new MatchingScopeLifetime(lifetimeScopeTag);
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerOwned<TService>()
        {
            return this.InstancePerOwned(typeof(TService));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerOwned<TService>(object serviceKey)
        {
            return this.InstancePerOwned(serviceKey, typeof(TService));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerOwned(Type serviceType)
        {
            return this.InstancePerMatchingLifetimeScope(new TypedService(serviceType));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerOwned(object serviceKey, Type serviceType)
        {
            return this.InstancePerMatchingLifetimeScope(new KeyedService(serviceKey, serviceType));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Keyed<TService>(object serviceKey)
        {
            return this.Keyed(serviceKey, typeof(TService));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Keyed(object serviceKey, Type serviceType)
        {
            if (serviceKey == null)
            {
                throw new ArgumentNullException("serviceKey");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.As(new Service[] { new KeyedService(serviceKey, serviceType) });
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Named<TService>(string serviceName)
        {
            return this.Named(serviceName, typeof(TService));
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Named(string serviceName, Type serviceType)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }
            return this.As(new Service[] { new KeyedService(serviceName, serviceType) });
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnActivated(Action<IActivatedEventArgs<TLimit>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            this.RegistrationData.ActivatedHandlers.Add(delegate (object s, ActivatedEventArgs<object> e) {
                handler(new ActivatedEventArgs<TLimit>(e.Context, e.Component, e.Parameters, (TLimit)e.Instance));
            });
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnActivating(Action<IActivatingEventArgs<TLimit>> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            this.RegistrationData.ActivatingHandlers.Add(delegate (object s, ActivatingEventArgs<object> e) {
                ActivatingEventArgs<TLimit> args = new ActivatingEventArgs<TLimit>(e.Context, e.Component, e.Parameters, (TLimit)e.Instance);
                handler(args);
                e.Instance = args.Instance;
            });
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnPreparing(Action<PreparingEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            this.RegistrationData.PreparingHandlers.Add(delegate (object s, PreparingEventArgs e) {
                handler(e);
            });
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OwnedByLifetimeScope()
        {
            this.RegistrationData.Ownership = InstanceOwnership.OwnedByLifetimeScope;
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PropertiesAutowired(PropertyWiringFlags wiringFlags)
        {
            AutowiringPropertyInjector injector = new AutowiringPropertyInjector();
            bool flag = PropertyWiringFlags.Default != (wiringFlags & PropertyWiringFlags.AllowCircularDependencies);
            bool preserveSetValues = PropertyWiringFlags.Default != (wiringFlags & PropertyWiringFlags.PreserveSetValues);
            if (flag)
            {
                this.RegistrationData.ActivatedHandlers.Add(delegate (object s, ActivatedEventArgs<object> e) {
                    injector.InjectProperties(e.Context, e.Instance, !preserveSetValues);
                });
            }
            else
            {
                this.RegistrationData.ActivatingHandlers.Add(delegate (object s, ActivatingEventArgs<object> e) {
                    injector.InjectProperties(e.Context, e.Instance, !preserveSetValues);
                });
            }
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> SingleInstance()
        {
            this.RegistrationData.Sharing = InstanceSharing.Shared;
            this.RegistrationData.Lifetime = new RootScopeLifetime();
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> WithMetadata<TMetadata>(Action<MetadataConfiguration<TMetadata>> configurationAction)
        {
            if (configurationAction == null)
            {
                throw new ArgumentNullException("configurationAction");
            }
            MetadataConfiguration<TMetadata> configuration = new MetadataConfiguration<TMetadata>();
            configurationAction(configuration);
            return this.WithMetadata(configuration.Properties);
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> WithMetadata(IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            foreach (KeyValuePair<string, object> pair in properties)
            {
                this.WithMetadata(pair.Key, pair.Value);
            }
            return this;
        }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> WithMetadata(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            this.RegistrationData.Metadata.Add(key, value);
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TActivatorData ActivatorData
        {
            get
            {
                return this._activatorData;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public TRegistrationStyle RegistrationStyle
        {
            get
            {
                return this._registrationStyle;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Autofac.Builder.RegistrationData RegistrationData
        {
            get
            {
                return this._registrationData;
            }
        }
    }
}


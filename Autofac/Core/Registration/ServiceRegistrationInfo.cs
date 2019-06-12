namespace Autofac.Core.Registration
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class ServiceRegistrationInfo
    {
        private readonly Service _service;
        private readonly LinkedList<IComponentRegistration> _implementations = new LinkedList<IComponentRegistration>();
        private Queue<IRegistrationSource> _sourcesToQuery;

        public ServiceRegistrationInfo(Service service)
        {
            this._service = service;
        }

        public void AddImplementation(IComponentRegistration registration, bool preserveDefaults)
        {
            if (preserveDefaults)
            {
                this._implementations.AddLast(registration);
            }
            else
            {
                bool any = this.Any;
                this._implementations.AddFirst(registration);
            }
        }

        public void BeginInitialization(IEnumerable<IRegistrationSource> sources)
        {
            this.IsInitialized = false;
            this._sourcesToQuery = new Queue<IRegistrationSource>(sources);
        }

        public void CompleteInitialization()
        {
            this.IsInitialized = true;
            this._sourcesToQuery = null;
        }

        public IRegistrationSource DequeueNextSource()
        {
            this.EnforceDuringInitialization();
            return this._sourcesToQuery.Dequeue();
        }

        private void EnforceDuringInitialization()
        {
            if (!this.IsInitializing)
            {
                throw new InvalidOperationException(ServiceRegistrationInfoResources.NotDuringInitialisation);
            }
        }

        public void Include(IRegistrationSource source)
        {
            if (this.IsInitialized)
            {
                this.BeginInitialization(new IRegistrationSource[] { source });
            }
            else if (this.IsInitializing)
            {
                this._sourcesToQuery.Enqueue(source);
            }
        }

        private void RequiresInitialization()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException(ServiceRegistrationInfoResources.NotInitialised);
            }
        }

        public bool ShouldRecalculateAdaptersOn(IComponentRegistration registration)
        {
            return this.IsInitialized;
        }

        public void SkipSource(IRegistrationSource source)
        {
            this.EnforceDuringInitialization();
            this._sourcesToQuery = new Queue<IRegistrationSource>(from rs in this._sourcesToQuery
                where rs != source
                select rs);
        }

        public bool TryGetRegistration(out IComponentRegistration registration)
        {
            this.RequiresInitialization();
            if (this.Any)
            {
                registration = this._implementations.First.Value;
                return true;
            }
            registration = null;
            return false;
        }

        public bool IsInitialized { get; private set; }

        public IEnumerable<IComponentRegistration> Implementations
        {
            get
            {
                this.RequiresInitialization();
                return this._implementations;
            }
        }

        public bool IsRegistered
        {
            get
            {
                this.RequiresInitialization();
                return this.Any;
            }
        }

        private bool Any
        {
            get
            {
                return (this._implementations.First != null);
            }
        }

        public bool IsInitializing
        {
            get
            {
                return (!this.IsInitialized && (this._sourcesToQuery != null));
            }
        }

        public bool HasSourcesToQuery
        {
            get
            {
                return (this.IsInitializing && (this._sourcesToQuery.Count != 0));
            }
        }
    }
}


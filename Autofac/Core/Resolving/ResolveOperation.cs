namespace Autofac.Core.Resolving
{
    using Autofac;
    using Autofac.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class ResolveOperation : IComponentContext, IResolveOperation
    {
        private readonly Stack<InstanceLookup> _activationStack = new Stack<InstanceLookup>();
        private ICollection<InstanceLookup> _successfulActivations;
        private readonly ISharingLifetimeScope _mostNestedLifetimeScope;
        private readonly CircularDependencyDetector _circularDependencyDetector = new CircularDependencyDetector();
        private int _callDepth;
        private bool _ended;

        public event EventHandler<ResolveOperationEndingEventArgs> CurrentOperationEnding;

        public event EventHandler<InstanceLookupBeginningEventArgs> InstanceLookupBeginning;

        public ResolveOperation(ISharingLifetimeScope mostNestedLifetimeScope)
        {
            this._mostNestedLifetimeScope = mostNestedLifetimeScope;
            this.ResetSuccessfulActivations();
        }

        private void CompleteActivations()
        {
            ICollection<InstanceLookup> is2 = this._successfulActivations;
            this.ResetSuccessfulActivations();
            foreach (InstanceLookup lookup in is2)
            {
                lookup.Complete();
            }
        }

        private void End(Exception exception)
        {
            if (!this._ended)
            {
                this._ended = true;
                EventHandler<ResolveOperationEndingEventArgs> currentOperationEnding = this.CurrentOperationEnding;
                if (currentOperationEnding != null)
                {
                    currentOperationEnding(this, new ResolveOperationEndingEventArgs(this, exception));
                }
            }
        }

        public object Execute(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            object obj2;
            try
            {
                obj2 = this.ResolveComponent(registration, parameters);
            }
            catch (DependencyResolutionException exception)
            {
                this.End(exception);
                throw;
            }
            catch (Exception exception2)
            {
                this.End(exception2);
                throw new DependencyResolutionException(ResolveOperationResources.ExceptionDuringResolve, exception2);
            }
            this.End(null);
            return obj2;
        }

        public object GetOrCreateInstance(ISharingLifetimeScope currentOperationScope, IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            if (currentOperationScope == null)
            {
                throw new ArgumentNullException("currentOperationScope");
            }
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (this._ended)
            {
                Exception innerException = null;
                throw new ObjectDisposedException(ResolveOperationResources.TemporaryContextDisposed, innerException);
            }
            this._circularDependencyDetector.CheckForCircularDependency(registration, this._activationStack, ++this._callDepth);
            InstanceLookup item = new InstanceLookup(registration, this, currentOperationScope, parameters);
            this._activationStack.Push(item);
            EventHandler<InstanceLookupBeginningEventArgs> instanceLookupBeginning = this.InstanceLookupBeginning;
            if (instanceLookupBeginning != null)
            {
                instanceLookupBeginning(this, new InstanceLookupBeginningEventArgs(item));
            }
            object obj2 = item.Execute();
            this._successfulActivations.Add(item);
            this._activationStack.Pop();
            if (this._activationStack.Count == 0)
            {
                this.CompleteActivations();
            }
            this._callDepth--;
            return obj2;
        }

        private void ResetSuccessfulActivations()
        {
            this._successfulActivations = new List<InstanceLookup>();
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return this.GetOrCreateInstance(this._mostNestedLifetimeScope, registration, parameters);
        }

        public IComponentRegistry ComponentRegistry
        {
            get
            {
                return this._mostNestedLifetimeScope.ComponentRegistry;
            }
        }
    }
}


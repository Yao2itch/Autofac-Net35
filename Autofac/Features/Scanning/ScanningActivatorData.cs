namespace Autofac.Features.Scanning
{
    using Autofac.Builder;
    using Core;
    using System;
    using System.Collections.Generic;

    public class ScanningActivatorData : ReflectionActivatorData
    {
        private readonly ICollection<Func<Type, bool>> _filters;
        private readonly ICollection<Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>> _configurationActions;
        private readonly ICollection<Action<IComponentRegistry>> _postScanningCallbacks;

        public ScanningActivatorData() : base(typeof(object))
        {
            this._filters = new List<Func<Type, bool>>();
            this._configurationActions = new List<Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>>();
            this._postScanningCallbacks = new List<Action<IComponentRegistry>>();
        }

        public ICollection<Func<Type, bool>> Filters
        {
            get
            {
                return this._filters;
            }
        }

        public ICollection<Action<Type, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>>> ConfigurationActions
        {
            get
            {
                return this._configurationActions;
            }
        }

        public ICollection<Action<IComponentRegistry>> PostScanningCallbacks
        {
            get
            {
                return this._postScanningCallbacks;
            }
        }
    }
}


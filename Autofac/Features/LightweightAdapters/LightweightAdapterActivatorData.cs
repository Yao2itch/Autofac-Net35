namespace Autofac.Features.LightweightAdapters
{
    using Autofac.Core;
    using System;
    using System.Collections.Generic;

    public class LightweightAdapterActivatorData
    {
        private readonly Service _fromService;
        private readonly Func<IComponentContext, IEnumerable<Parameter>, object, object> _adapter;

        public LightweightAdapterActivatorData(Service fromService, Func<IComponentContext, IEnumerable<Parameter>, object, object> adapter)
        {
            this._fromService = fromService;
            this._adapter = adapter;
        }

        public Func<IComponentContext, IEnumerable<Parameter>, object, object> Adapter
        {
            get
            {
                return this._adapter;
            }
        }

        public Service FromService
        {
            get
            {
                return this._fromService;
            }
        }
    }
}


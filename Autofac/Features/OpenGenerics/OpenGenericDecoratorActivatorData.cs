namespace Autofac.Features.OpenGenerics
{
    using Autofac.Builder;
    using Autofac.Core;
    using System;

    public class OpenGenericDecoratorActivatorData : ReflectionActivatorData
    {
        private readonly IServiceWithType _fromService;

        public OpenGenericDecoratorActivatorData(Type implementor, IServiceWithType fromService) : base(implementor)
        {
            if (fromService == null)
            {
                throw new ArgumentNullException("fromService");
            }
            if (!fromService.ServiceType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(string.Format(OpenGenericDecoratorActivatorDataResources.DecoratedServiceIsNotOpenGeneric, fromService));
            }
            this._fromService = fromService;
        }

        public IServiceWithType FromService
        {
            get
            {
                return this._fromService;
            }
        }
    }
}


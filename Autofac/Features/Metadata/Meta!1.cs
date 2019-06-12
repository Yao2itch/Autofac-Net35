namespace Autofac.Features.Metadata
{
    using System;
    using System.Collections.Generic;

    public class Meta<T>
    {
        private readonly T _value;
        private readonly IDictionary<string, object> _metadata;

        public Meta(T value, IDictionary<string, object> metadata)
        {
            this._value = value;
            this._metadata = metadata;
        }

        public T Value
        {
            get
            {
                return this._value;
            }
        }

        public IDictionary<string, object> Metadata
        {
            get
            {
                return this._metadata;
            }
        }
    }
}


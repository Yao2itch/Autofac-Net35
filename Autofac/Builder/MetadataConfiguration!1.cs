namespace Autofac.Builder
{
    using Autofac.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class MetadataConfiguration<TMetadata>
    {
        private readonly IDictionary<string, object> _properties;

        public MetadataConfiguration()
        {
            this._properties = new Dictionary<string, object>();
        }

        public MetadataConfiguration<TMetadata> For<TProperty>(Expression<Func<TMetadata, TProperty>> propertyAccessor, TProperty value)
        {
            if (propertyAccessor == null)
            {
                throw new ArgumentNullException("propertyAccessor");
            }
            string name = ReflectionExtensions.GetProperty<TMetadata, TProperty>(propertyAccessor).Name;
            this._properties.Add(name, value);
            return (MetadataConfiguration<TMetadata>) this;
        }

        internal IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
                return this._properties;
            }
        }
    }
}


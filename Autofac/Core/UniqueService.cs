namespace Autofac.Core
{
    using System;

    public sealed class UniqueService : Service
    {
        private Guid _id;

        public UniqueService() : this(Guid.NewGuid())
        {
        }

        public UniqueService(Guid id)
        {
            this._id = id;
        }

        public override bool Equals(object obj)
        {
            UniqueService service = obj as UniqueService;
            return ((service != null) && (this._id == service._id));
        }

        public override int GetHashCode()
        {
            return this._id.GetHashCode();
        }

        public override string Description
        {
            get
            {
                return this._id.ToString();
            }
        }
    }
}


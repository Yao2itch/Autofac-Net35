namespace Autofac.Util
{
    using System;

    internal class ReleaseAction : Disposable
    {
        private readonly Action _action;

        public ReleaseAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            this._action = action;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._action();
            }
            base.Dispose(disposing);
        }
    }
}


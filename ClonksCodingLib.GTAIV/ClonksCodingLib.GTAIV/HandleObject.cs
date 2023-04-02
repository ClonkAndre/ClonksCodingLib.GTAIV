using System;

namespace CCL.GTAIV
{
    public abstract class HandleObject : IDisposable
    {

        #region Variables and Properties
        // Variables
        private bool _disposed;
        private int _handle;

        // Properties
        /// <summary>
        /// Gets if this native object was disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
            private set { _disposed = value; }
        }
        /// <summary>
        /// Gets the handle of this native object.
        /// </summary>
        public int Handle
        {
            get {
                if (IsDisposed)
                    return 0;

                return _handle;
            }
            private set { _handle = value; }
        }
        /// <summary>
        /// Returns <see langword="true"/> if this handle object was not disposed and the <see cref="Handle"/> is not 0.
        /// </summary>
        public bool IsValid
        {
            get { return !IsDisposed && Handle != 0; }
        }
        #endregion

        #region Constructor
        internal HandleObject(int handle)
        {
            Handle = handle;
        }
        #endregion

        /// <summary>
        /// Disposes this native object.
        /// </summary>
        public virtual void Dispose()
        {
            IsDisposed = true;
            Handle = 0;
        }

        /// <summary>
        /// Checks if this native object still exists.
        /// </summary>
        /// <returns>True if native object exists. Otherwise, false.</returns>
        public abstract bool Exists();

        /// <summary>
        /// Deletes this native object from the world without disposing.
        /// </summary>
        public virtual void Delete()
        {
            Handle = 0;
        }

        /// <summary>
        /// Gets the handle of this object.
        /// </summary>
        /// <returns>The handle if this object.</returns>
        public override int GetHashCode()
        {
            return Handle;
        }

    }
}

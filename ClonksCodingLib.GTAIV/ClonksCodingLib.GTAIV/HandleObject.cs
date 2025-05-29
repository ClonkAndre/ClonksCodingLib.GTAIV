namespace CCL.GTAIV
{
    /// <summary>
    /// A <see cref="HandleObject"/> is an entity which has a <b>handle</b> like the <see cref="IVSDKDotNet.IVPed"/>, <see cref="IVSDKDotNet.IVVehicle"/> or the <see cref="NativeBlip"/>.<br/>
    /// Most native functions require a <b>handle</b> like the <see cref="IVSDKDotNet.Native.Natives.DOES_CHAR_EXIST(int)"/> native, which requries the <b>handle</b> of a <see cref="IVSDKDotNet.IVPed"/>.
    /// </summary>
    public abstract class HandleObject
    {

        #region Variables and Properties
        // Variables
        private bool _deleted;
        private int _handle;

        // Properties
        /// <summary>
        /// Gets if this native object was deleted.
        /// </summary>
        public bool IsDeleted
        {
            get { return _deleted; }
            private set { _deleted = value; }
        }

        /// <summary>
        /// Returns <see langword="true"/> if this handle object was not deleted and the <see cref="Handle"/> is not 0.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !IsDeleted && Handle != 0;
            }
        }

        /// <summary>
        /// Gets the handle of this native object.
        /// </summary>
        public int Handle
        {
            get
            {
                if (IsDeleted)
                    return 0;

                return _handle;
            }
            private set { _handle = value; }
        }
        #endregion

        #region Constructor
        internal HandleObject(int handle)
        {
            Handle = handle;
        }
        #endregion

        internal void SetHandle(int newHandle)
        {
            Handle = newHandle;
        }

        /// <summary>
        /// Deletes this native object.
        /// </summary>
        public virtual void Delete()
        {
            IsDeleted = true;
            Handle = 0;
        }

        /// <summary>
        /// Checks if this native object still exists.
        /// </summary>
        /// <returns>True if native object exists. Otherwise, false.</returns>
        public abstract bool Exists();

        /// <summary>
        /// Gets the handle of this native object.
        /// </summary>
        /// <returns>The handle of this native object.</returns>
        public override int GetHashCode()
        {
            return Handle;
        }

    }
}

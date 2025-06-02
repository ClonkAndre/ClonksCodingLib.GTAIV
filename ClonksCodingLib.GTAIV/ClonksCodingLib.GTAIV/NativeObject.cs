using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    // TODO: Finish!
    /// <summary>
    /// Gives you easy access to native functions that involve objects.
    /// </summary>
    public class NativeObject : HandleObject
    {

        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativeObject"/> class with an existing handle.
        /// </summary>
        /// <param name="handle">The handle of an already existing object.</param>
        public NativeObject(int handle) : base(handle)
        {

        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Delete()
        {
            if (Exists())
            {
                int handle = Handle;
                DELETE_OBJECT(ref handle);
            }

            base.Delete();
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_OBJECT_EXIST(Handle);
        }
        #endregion

    }
}

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
    /// Gives you easy access to native functions that involve vehicles.
    /// </summary>
    public class NativeVehicle : HandleObject
    {

        #region Variables

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativeVehicle"/> class with an existing handle.
        /// </summary>
        /// <param name="handle">The handle of an already existing vehicle.</param>
        public NativeVehicle(int handle) : base(handle)
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
                DELETE_CAR(ref handle);
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

            return DOES_VEHICLE_EXIST(Handle);
        }
        #endregion

    }
}

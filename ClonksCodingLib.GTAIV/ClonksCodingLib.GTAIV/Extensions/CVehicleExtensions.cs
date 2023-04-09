using System;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="CVehicle"/> class.
    /// </summary>
    public static class CVehicleExtensions
    {
        #region Methods

        #endregion

        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="CVehicle"/>.
        /// <para>Can be used for calling native functions like <see cref="IS_CAR_DEAD(int)"/> which requires a vehicle handle.</para>
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the handle of this <see cref="CVehicle"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this CVehicle veh)
        {
            if (veh == null)
                return 0;

            return (int)CPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());
        }

        /// <summary>
        /// Gets the <see cref="Rectangle3D"/> (or bounds) of this <see cref="CVehicle"/> with their current model.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the <see cref="Rectangle3D"/> of this <see cref="CVehicle"/> is returned. Otherwise, <see cref="Rectangle3D.Empty()"/> is returned.</returns>
        public static Rectangle3D GetModelRect3D(this CVehicle veh)
        {
            if (veh == null)
                return Rectangle3D.Empty();

            int handle = GetHandle(veh);
            GET_CAR_MODEL(handle, out uint pModel);

            if (pModel == 0)
                return Rectangle3D.Empty();

            GET_MODEL_DIMENSIONS(pModel, out Vector3 min, out Vector3 max);
            return new Rectangle3D(veh.Matrix.pos, max);
        }
        #endregion
    }
}

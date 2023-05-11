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
        public static void ApplyForce(this CVehicle veh, Vector3 direction, Vector3 rotation)
        {
            if (veh == null)
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public static void ApplyForce(this CVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;

            ApplyForce(veh, direction, Vector3.Zero);
        }

        public static void ApplyForceRelative(this CVehicle veh, Vector3 direction, Vector3 rotation)
        {
            if (veh == null)
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public static void ApplyForceRelative(this CVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;

            ApplyForceRelative(veh, direction, Vector3.Zero);
        }

        public static void SetCurrentRoom(this CVehicle veh, NativeRoom room)
        {
            if (veh == null)
                return;
            if (room == null)
                return;

            SET_ROOM_FOR_CAR_BY_KEY(GetHandle(veh), (uint)room.Room);
        }

        public static void Delete(this CVehicle veh)
        {
            if (veh == null)
                return;

            CPools.DeleteCar(GetHandle(veh));
        }
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

        public static float GetSpeed(this CVehicle veh)
        {
            if (veh == null)
                return 0f;

            GET_CAR_SPEED(GetHandle(veh), out float speed);
            return speed;
        }
        public static Vector3 GetSpeedVector(this CVehicle veh, bool unknownFalse = false)
        {
            if (veh == null)
                return Vector3.Zero;

            GET_CAR_SPEED_VECTOR(GetHandle(veh), out Vector3 vec, unknownFalse);
            return vec;
        }

        public static NativeRoom GetCurrentRoom(this CVehicle veh)
        {
            if (veh == null)
                return null;

            return NativeRoom.FromVehicle(veh);
        }

        public static bool Exists(this CVehicle veh)
        {
            if (veh == null)
                return false;

            return DOES_VEHICLE_EXIST(GetHandle(veh));
        }

        /// <summary>
        /// Attaches a <see cref="NativeBlip"/> to this <see cref="CVehicle"/>.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the attached <see cref="NativeBlip"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeBlip AttachBlip(this CVehicle veh)
        {
            if (veh == null)
                return null;

            return NativeBlip.AddBlip(veh);
        }
        #endregion
    }
}

using System;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="IVVehicle"/> class.
    /// </summary>
    public static class IVVehicleExtensions
    {
        #region Methods
        public static void ApplyForce(this IVVehicle veh, Vector3 direction, Vector3 rotation)
        {
            if (veh == null)
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public static void ApplyForce(this IVVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;

            ApplyForce(veh, direction, Vector3.Zero);
        }

        public static void ApplyForceRelative(this IVVehicle veh, Vector3 direction, Vector3 rotation)
        {
            if (veh == null)
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public static void ApplyForceRelative(this IVVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;

            ApplyForceRelative(veh, direction, Vector3.Zero);
        }

        public static void SetCurrentRoom(this IVVehicle veh, NativeRoom room)
        {
            if (veh == null)
                return;
            if (room == null)
                return;

            SET_ROOM_FOR_CAR_BY_KEY(GetHandle(veh), (uint)room.Room);
        }

        public static void PlaceOnGroundProperly(this IVVehicle veh)
        {
            if (veh == null)
                return;

            SET_CAR_ON_GROUND_PROPERLY(GetHandle(veh));
        }

        public static void SetAsMissionVehicle(this IVVehicle veh)
        {
            if (veh == null)
                return;

            SET_CAR_AS_MISSION_CAR(GetHandle(veh));
        }
        public static void MarkAsNoLongerNeeded(this IVVehicle veh)
        {
            if (veh == null)
                return;

            MARK_CAR_AS_NO_LONGER_NEEDED(GetHandle(veh));
        }
        public static void Delete(this IVVehicle veh)
        {
            if (veh == null)
                return;

            int handle = GetHandle(veh);
            DELETE_CAR(ref handle);
        }

        public static unsafe void SoundCarHorn(this IVVehicle veh, uint time)
        {
            if (veh == null)
                return;

            *(uint*)(veh.GetUIntPtr().ToUInt32() + 0x133C) = time;
        }
        public static unsafe void SetVehIndicator(this IVVehicle veh, VehicleIndicator indicator)
        {
            if (veh == null)
                return;

            *(uint*)(veh.GetUIntPtr().ToUInt32() + 3947 + (int)indicator) = (uint)177;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="IVVehicle"/>.
        /// <para>Can be used for calling native functions like <see cref="IS_CAR_DEAD(int)"/> which requires a vehicle handle.</para>
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the handle of this <see cref="IVVehicle"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this IVVehicle veh)
        {
            if (veh == null)
                return 0;
            
            return (int)IVPools.GetVehiclePool().GetIndex(veh.GetUIntPtr());
        }

        /// <summary>
        /// Gets the <see cref="Rectangle3D"/> (or bounds) of this <see cref="IVVehicle"/> with their current model.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the <see cref="Rectangle3D"/> of this <see cref="IVVehicle"/> is returned. Otherwise, <see cref="Rectangle3D.Empty()"/> is returned.</returns>
        public static Rectangle3D GetModelRect3D(this IVVehicle veh)
        {
            if (veh == null)
                return Rectangle3D.Empty();

            int handle = GetHandle(veh);
            GET_CAR_MODEL(handle, out uint pModel);

            if (pModel == 0)
                return Rectangle3D.Empty();

            GET_MODEL_DIMENSIONS(pModel, out Vector3 min, out Vector3 max);
            return new Rectangle3D(veh.Matrix.Pos, max);
        }

        public static float GetSpeed(this IVVehicle veh)
        {
            if (veh == null)
                return 0f;

            GET_CAR_SPEED(GetHandle(veh), out float speed);
            return speed;
        }
        public static Vector3 GetSpeedVector(this IVVehicle veh, bool unknownFalse = false)
        {
            if (veh == null)
                return Vector3.Zero;

            GET_CAR_SPEED_VECTOR(GetHandle(veh), out Vector3 vec, unknownFalse);
            return vec;
        }

        public static float GetHeading(this IVVehicle veh)
        {
            if (veh == null)
                return 0f;

            GET_CAR_HEADING(veh.GetHandle(), out float heading);
            return heading;
        }

        public static NativeRoom GetCurrentRoom(this IVVehicle veh)
        {
            if (veh == null)
                return null;

            return NativeRoom.FromVehicle(veh);
        }

        public static bool Exists(this IVVehicle veh)
        {
            if (veh == null)
                return false;

            return DOES_VEHICLE_EXIST(GetHandle(veh));
        }

        public static bool IsDead(this IVVehicle veh)
        {
            if (veh == null)
                return false;
            
            return IS_CAR_DEAD(GetHandle(veh));
        }
        public static bool IsDriveable(this IVVehicle veh)
        {
            if (veh == null)
                return false;

            return IS_VEH_DRIVEABLE(GetHandle(veh));
        }

        /// <summary>
        /// Attaches a <see cref="NativeBlip"/> to this <see cref="IVVehicle"/>.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>If successful, the attached <see cref="NativeBlip"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeBlip AttachBlip(this IVVehicle veh)
        {
            if (veh == null)
                return null;

            return NativeBlip.AddBlip(veh);
        }

        public static unsafe uint GetVehIndicatorState(this IVVehicle veh, VehicleIndicator indicator)
        {
            if (veh == null)
                return 0;

            return *(uint*)(veh.GetUIntPtr().ToUInt32() + 3947 + (int)indicator);
        }
        public static unsafe uint GetCarHornStatus(this IVVehicle veh)
        {
            if (veh == null)
                return 0;

            return *(uint*)(veh.GetUIntPtr().ToUInt32() + 0x133C);
        }
        #endregion
    }
}

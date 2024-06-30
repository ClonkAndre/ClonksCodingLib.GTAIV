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
            if (!Exists(veh))
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public static void ApplyForce(this IVVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;
            
            ApplyForce(veh, direction, Vector3.Zero);
        }

        public static void ApplyForceRelative(this IVVehicle veh, Vector3 direction, Vector3 rotation)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            APPLY_FORCE_TO_CAR(GetHandle(veh), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public static void ApplyForceRelative(this IVVehicle veh, Vector3 direction)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            ApplyForceRelative(veh, direction, Vector3.Zero);
        }

        public static void SetCurrentRoom(this IVVehicle veh, NativeRoom room)
        {
            if (veh == null)
                return;
            if (room == null)
                return;
            if (!Exists(veh))
                return;

            SET_ROOM_FOR_CAR_BY_KEY(GetHandle(veh), (uint)room.Room);
        }
        public static void SetHeading(this IVVehicle veh, float heading)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;
            
            SET_CAR_HEADING(GetHandle(veh), heading);
        }

        public static void PlaceOnGroundProperly(this IVVehicle veh)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            SET_CAR_ON_GROUND_PROPERLY(GetHandle(veh));
        }

        public static void SetAsMissionVehicle(this IVVehicle veh)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            SET_CAR_AS_MISSION_CAR(GetHandle(veh));
        }
        public static void MarkAsNoLongerNeeded(this IVVehicle veh)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            MARK_CAR_AS_NO_LONGER_NEEDED(GetHandle(veh));
        }

        public static void Delete(this IVVehicle veh)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            int handle = GetHandle(veh);
            DELETE_CAR(ref handle);
        }

        public static void Explode(this IVVehicle veh, bool addExplosion, bool maybeKeepDamageEntity = false)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            EXPLODE_CAR(GetHandle(veh), addExplosion, maybeKeepDamageEntity);
        }

        public static unsafe void SoundCarHorn(this IVVehicle veh, uint time)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
                return;

            *(uint*)(veh.GetUIntPtr().ToUInt32() + 0x133C) = time;
        }
        public static unsafe void SetVehIndicator(this IVVehicle veh, VehicleIndicator indicator)
        {
            if (veh == null)
                return;
            if (!Exists(veh))
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
        /// Checks if this <see cref="IVVehicle"/> exists.
        /// </summary>
        /// <param name="veh"></param>
        /// <returns>True if this <see cref="IVVehicle"/> exists. Otherwise, false.</returns>
        public static bool Exists(this IVVehicle veh)
        {
            if (veh == null)
                return false;

            return DOES_VEHICLE_EXIST(GetHandle(veh));
        }

        public static bool IsRequiredForMission(this IVVehicle veh)
        {
            if (veh == null)
                return false;
            if (!Exists(veh))
                return false;

            return IS_CAR_A_MISSION_CAR(GetHandle(veh));
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
            if (!Exists(veh))
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
            if (!Exists(veh))
                return 0f;

            GET_CAR_SPEED(GetHandle(veh), out float speed);
            return speed;
        }
        public static Vector3 GetSpeedVector(this IVVehicle veh, bool unknownFalse = false)
        {
            if (veh == null)
                return Vector3.Zero;
            if (!Exists(veh))
                return Vector3.Zero;

            GET_CAR_SPEED_VECTOR(GetHandle(veh), out Vector3 vec, unknownFalse);
            return vec;
        }

        public static float GetHeading(this IVVehicle veh)
        {
            if (veh == null)
                return 0f;
            if (!Exists(veh))
                return 0f;

            GET_CAR_HEADING(veh.GetHandle(), out float heading);
            return heading;
        }

        public static NativeRoom GetCurrentRoom(this IVVehicle veh)
        {
            if (veh == null)
                return null;
            if (!Exists(veh))
                return null;

            return NativeRoom.FromVehicle(veh);
        }

        public static bool IsDead(this IVVehicle veh)
        {
            if (veh == null)
                return false;
            if (!Exists(veh))
                return false;

            return IS_CAR_DEAD(GetHandle(veh));
        }
        public static bool IsDriveable(this IVVehicle veh)
        {
            if (veh == null)
                return false;
            if (!Exists(veh))
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
            if (!Exists(veh))
                return null;

            return NativeBlip.AddBlip(veh);
        }

        public static int GetMaximumNumberOfPassengers(this IVVehicle veh)
        {
            if (veh == null)
                return 0;
            if (!Exists(veh))
                return 0;

            GET_MAXIMUM_NUMBER_OF_PASSENGERS(GetHandle(veh), out int max);
            return max;
        }

        public static bool IsSeatFree(this IVVehicle veh, VehicleSeat seat)
        {
            if (veh == null)
                return false;
            if (!Exists(veh))
                return false;

            if (seat <= VehicleSeat.None)
                return false;

            int handle = GetHandle(veh);
            int maxPassengers = GetMaximumNumberOfPassengers(veh);

            switch (seat)
            {
                case VehicleSeat.AnyPassengerSeat:

                    for (int i = 0; i < maxPassengers; i++)
                    {
                        if (IsSeatFree(veh, (VehicleSeat)i))
                            return true;
                    }
                    return false;

                case VehicleSeat.Driver:

                    GET_DRIVER_OF_CAR(handle, out int ped);
                    return ped == 0;

                default:
                    return IS_CAR_PASSENGER_SEAT_FREE(handle, (uint)seat);
            }
        }

        public static bool CanVehicleSeeVehicle(this IVVehicle source, int targetVehicle, float sourceVehicleViewDistance = 50f, float sourceVehicleFOV = 90f)
        {
            if (source == null)
                return false;
            if (targetVehicle <= 0)
                return false;
            if (!Exists(source))
                return false;
            if (!DOES_VEHICLE_EXIST(targetVehicle))
                return false;

            // Get the position of the source vehicle
            int sourceCarHandle = source.GetHandle();
            GET_CAR_MODEL(sourceCarHandle, out uint model);
            GET_MODEL_DIMENSIONS(model, out Vector3 min, out Vector3 max);
            GET_OFFSET_FROM_CAR_IN_WORLD_COORDS(sourceCarHandle, new Vector3(0f, 0f, max.Z + 0.1f), out Vector3 sourceCarInfrontPos);

            // Get the position of the target vehicle
            GET_CAR_MODEL(targetVehicle, out model);
            GET_MODEL_DIMENSIONS(model, out min, out max);
            GET_OFFSET_FROM_CAR_IN_WORLD_COORDS(targetVehicle, new Vector3(0f, 0f, max.Z + 0.1f), out Vector3 targetVehiclePosition);

            Vector3 toTarget = targetVehiclePosition - sourceCarInfrontPos;

            // Check if within view distance
            if (Vector3.Distance(source.Matrix.Pos, targetVehiclePosition) > sourceVehicleViewDistance)
                return false;

            // Normalize the vector to target
            toTarget = Vector3.Normalize(toTarget);

            // Calculate the angle between the forward direction and the direction to the target
            float dotProduct = Vector3.Dot(Helper.HeadingToDirection(source.GetHeading()), toTarget);
            float angleToTarget = (float)Math.Acos(dotProduct) * (180f / (float)Math.PI); // Convert to degrees

            // Check if within field of view
            if (angleToTarget > sourceVehicleFOV / 2f)
                return false;

            // Check if source vehicle has a clear line-of-sight to the target vehicle
            // source.Matrix.Pos
            return !IVWorld.ProcessLineOfSight(sourceCarInfrontPos, targetVehiclePosition, out IVLineOfSightResults res, 1);
        }
        public static bool CanVehicleSeeVehicle(this IVVehicle source, IVVehicle targetVehicle, float sourceVehicleViewDistance = 50f, float sourceVehicleFOV = 90f)
        {
            if (targetVehicle == null)
                return false;
            if (!targetVehicle.Exists())
                return false;

            return CanVehicleSeeVehicle(source, targetVehicle.GetHandle(), sourceVehicleViewDistance, sourceVehicleFOV);
        }

        public static unsafe uint GetVehIndicatorState(this IVVehicle veh, VehicleIndicator indicator)
        {
            if (veh == null)
                return 0;
            if (!Exists(veh))
                return 0;

            return *(uint*)(veh.GetUIntPtr().ToUInt32() + 3947 + (int)indicator);
        }
        public static unsafe uint GetCarHornStatus(this IVVehicle veh)
        {
            if (veh == null)
                return 0;
            if (!Exists(veh))
                return 0;

            return *(uint*)(veh.GetUIntPtr().ToUInt32() + 0x133C);
        }
        #endregion
    }
}

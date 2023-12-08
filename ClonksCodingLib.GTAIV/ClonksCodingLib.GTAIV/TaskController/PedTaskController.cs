using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV.TaskController
{
    public class PedTaskController
    {
        #region Variables and Properties
        // Variables
        /// <summary>1 Hour</summary>
        public const int MAX_DURATION = 3600000;

        private IVPed ped;
        private int handle;

        // Properties
        public bool AlwaysKeepTask
        {
            set
            {
                if (ped == null)
                    return;

                SET_CHAR_KEEP_TASK(handle, value);
            }
        }
        #endregion

        #region Constructor
        internal PedTaskController(IVPed targetPed)
        {
            ped = targetPed;
            handle = ped.GetHandle();
        }
        internal PedTaskController(int pedHandle)
        {
            handle = pedHandle;
        }
        #endregion

        public void ClearAll()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_TASKS(handle);
        }
        public void ClearAllImmediately()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_TASKS_IMMEDIATELY(handle);
        }
        public void ClearSecondary()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_SECONDARY_TASK(handle);
        }

        public void AimAt(Vector3 target, uint duration)
        {
            if (ped == null)
                return;

            _TASK_AIM_GUN_AT_COORD(handle, target.X, target.Y, target.Z, duration);
        }
        public void AimAt(IVPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_AIM_GUN_AT_CHAR(handle, target.GetHandle(), duration);
        }
        public void CruiseWithVehicle(IVVehicle veh, float speedMph, bool obeyTrafficLaws)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            _TASK_CAR_DRIVE_WANDER(handle, veh.GetHandle(), speedMph, (uint)(obeyTrafficLaws ? 1 : 2));
        }
        public void Die()
        {
            if (ped == null)
                return;

            _TASK_DIE(handle);
        }
        public void DrivePointRoute(IVVehicle veh, float speed, Vector3[] routePoints)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;
            if (routePoints.Length == 0)
                return;

            _TASK_FLUSH_ROUTE();
            for (int i = 0; i < routePoints.Length; i++)
            {
                _TASK_EXTEND_ROUTE(routePoints[i]);
            }
            _TASK_DRIVE_POINT_ROUTE(handle, veh.GetHandle(), speed);
        }
        public void DriveTo(IVVehicle veh, IVPed targetPed, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;
            if (targetPed == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_PED_TARGET_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), targetPed.GetHandle(), 4, (int)speedMph, obeyTrafficLaws ? 1 : 2, 5, 10);
            else
                _TASK_CAR_MISSION_PED_TARGET(handle, veh.GetHandle(), targetPed.GetHandle(), 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void DriveTo(IVVehicle veh, IVVehicle targetVeh, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;
            if (targetVeh == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), (uint)targetVeh.GetHandle(), 1, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 10, 5);
            else
            {
                _TASK_CAR_MISSION(handle, veh.GetHandle(), (uint)targetVeh.GetHandle(), 1, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 10, 5);
            }
        }
        public void DriveTo(IVVehicle veh, Vector3 target, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_COORS_TARGET_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
            else
                _TASK_CAR_MISSION_COORS_TARGET(handle, veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void EnterVehicle()
        {
            if (ped == null)
                return;

            _TASK_ENTER_CAR_AS_PASSENGER(handle, 0, 0, 2);
        }
        public void EnterVehicle(IVVehicle veh, uint seat)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (seat == 0)
                _TASK_ENTER_CAR_AS_DRIVER(handle, veh.GetHandle(), 0);
            else
                _TASK_ENTER_CAR_AS_PASSENGER(handle, veh.GetHandle(), 0, seat);
        }
        public void FightAgainst(IVPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_COMBAT_TIMED(handle, target.GetHandle(), duration);
        }
        public void FightAgainst(IVPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_COMBAT(handle, target.GetHandle());
        }
        public void FightAgainstHatedTargets(float radius, uint duration)
        {
            if (ped == null)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR_TIMED(handle, radius, duration);
        }
        public void FightAgainstHatedTargets(float radius)
        {
            if (ped == null)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR(handle, radius);
        }
        public void FleeFromChar(IVPed target, bool onPavements, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            if (onPavements)
                _TASK_SMART_FLEE_CHAR_PREFERRING_PAVEMENTS(handle, target.GetHandle(), 100f, duration);
            else
                _TASK_SMART_FLEE_CHAR(handle, target.GetHandle(), 100f, duration);
        }
        public void FleeFromChar(IVPed target, bool onPavements)
        {
            if (ped == null)
                return;

            FleeFromChar(target, onPavements, MAX_DURATION);
        }
        public void FleeFromChar(IVPed target)
        {
            if (ped == null)
                return;

            FleeFromChar(target, false, MAX_DURATION);
        }
        public void GoTo(IVPed target, float offsetRight, float offsetFront, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), duration, offsetRight, offsetFront);
        }
        public void GoTo(IVPed target, float offsetRight, float offsetFront)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), MAX_DURATION, offsetRight, offsetFront);
        }
        public void GoTo(IVPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), MAX_DURATION, 0f, 0f);
        }
        public void GoTo(Vector3 pos, bool ignorePaths)
        {
            if (ped == null)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(handle, pos.X, pos.Y, pos.Z, 2, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(handle, pos.X, pos.Y, pos.Z, 2, 0, 1f);
        }
        public void GoTo(Vector3 pos)
        {
            if (ped == null)
                return;

            GoTo(pos, false);
        }
        public void GuardCurrentPosition()
        {
            if (ped == null)
                return;

            _TASK_GUARD_CURRENT_POSITION(handle, 15f, 10f, 1);
        }
        public void HandsUp(uint duration)
        {
            if (ped == null)
                return;

            _TASK_HANDS_UP(handle, duration);
        }
        public void LandHelicopter(IVVehicle veh, Vector3 pos)
        {
            if (ped == null)
                return;

            _TASK_HELI_MISSION(handle, veh.GetHandle(), 0, 0, pos.X, pos.Y, pos.Z, 5, 0f, 0, -1f, 0, 0);
        }
        public void LeaveVehicle(IVVehicle veh, bool closeDoor)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (closeDoor)
                _TASK_LEAVE_CAR(handle, veh.GetHandle());
            else
                _TASK_LEAVE_CAR_DONT_CLOSE_DOOR(handle, veh.GetHandle());
        }
        public void LeaveVehicle()
        {
            if (ped == null)
                return;

            _TASK_LEAVE_ANY_CAR(handle);
        }
        public void LeaveVehicleImmediately(IVVehicle veh)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            _TASK_LEAVE_CAR_IMMEDIATELY(handle, veh.GetHandle());
        }
        public void LookAt(Vector3 target, uint duration)
        {
            if (ped == null)
                return;

            _TASK_LOOK_AT_COORD(handle, target.X, target.Y, target.Z, duration, 0);
        }
        public void LookAt(IVObject target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_OBJECT(handle, target.GetHandle(), duration, 0);
        }
        public void LookAt(IVVehicle target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_VEHICLE(handle, target.GetHandle(), duration, 0);
        }
        public void LookAt(IVPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_CHAR(handle, target.GetHandle(), duration, 0);
        }
        public void PlayAnimation(string animSet, string animName, float speed, int unknown, AnimationFlags flags)
        {
            if (ped == null)
                return;

            _TASK_PLAY_ANIM_WITH_FLAGS(handle, animName, animSet, speed, unknown, (int)flags);
        }
        public void PlayAnimation(string animSet, string animName, float speed, AnimationFlags flags)
        {
            PlayAnimation(animSet, animName, speed, -1, flags);
        }
        public void PlayAnimation(string animSet, string animName, float speed)
        {
            PlayAnimation(animSet, animName, speed, -1, AnimationFlags.None);
        }
        public void PutAwayMobilePhone()
        {
            if (ped == null)
                return;

            _TASK_USE_MOBILE_PHONE(handle, false);
        }
        public void RunTo(Vector3 pos, bool ignorePaths)
        {
            if (ped == null)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(handle, pos.X, pos.Y, pos.Z, 4, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(handle, pos.X, pos.Y, pos.Z, 4, 0, 1f);
        }
        public void RunTo(Vector3 pos)
        {
            if (ped == null)
                return;

            RunTo(pos, false);
        }
        public void ShootAt(IVPed target, int duration, ShootMode mode)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_SHOOT_AT_CHAR(handle, target.GetHandle(), duration, (int)mode);
        }
        public void ShootAt(IVPed target, ShootMode mode)
        {
            if (ped == null)
                return;

            ShootAt(target, -1, mode);
        }
        public void StandStill(int duration)
        {
            if (ped == null)
                return;
            if (duration < 0)
                duration = MAX_DURATION;

            _TASK_STAND_STILL(handle, duration);
        }
        public void SwapWeapon(eWeaponType weapon)
        {
            if (ped == null)
                return;

            _TASK_SWAP_WEAPON(handle, (uint)weapon);
        }
        /// <summary>
        /// Example scenario: Vehicle_LookingInBoot
        /// </summary>
        /// <param name="scenarioName">The name of the scenario to start.</param>
        /// <param name="pos">The position of the scenario to start?</param>
        public void StartScenario(string scenarioName, Vector3 pos)
        {
            if (ped == null)
                return;

            _TASK_START_SCENARIO_AT_POSITION(handle, scenarioName, pos, 0);
        }
        public void TurnTo(Vector3 pos)
        {
            if (ped == null)
                return;

            _TASK_TURN_CHAR_TO_FACE_COORD(handle, pos.X, pos.Y, pos.Z);
        }
        public void TurnTo(IVPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_TURN_CHAR_TO_FACE_CHAR(handle, target.GetHandle());
        }
        public void UseMobilePhone()
        {
            if (ped == null)
                return;

            _TASK_USE_MOBILE_PHONE(handle, true);
        }
        public void UseMobilePhone(uint duration)
        {
            if (ped == null)
                return;

            _TASK_USE_MOBILE_PHONE_TIMED(handle, duration);
        }
        public void Wait(uint duration)
        {
            if (ped == null)
                return;

            _TASK_PAUSE(handle, duration);
        }
        public void WanderAround()
        {
            if (ped == null)
                return;

            _TASK_WANDER_STANDARD(handle);
        }
        public void WarpIntoVehicle(IVVehicle veh, uint seat)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (seat == 0)
                _TASK_WARP_CHAR_INTO_CAR_AS_DRIVER(handle, veh.GetHandle());
            else
                _TASK_WARP_CHAR_INTO_CAR_AS_PASSENGER(handle, veh.GetHandle(), seat);
        }

        public void PerformSequence(NativeTaskSequence sequence)
        {
            if (sequence == null)
                return;

            sequence.Perform(ped);
        }

    }
}

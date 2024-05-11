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
        internal static readonly PedTaskController TempTaskController = new PedTaskController(0);

        /// <summary>1 Hour</summary>
        public const int MAX_DURATION = 3600000;

        private IVPed thePed;
        private int handle;
        private bool wasCreatedForTaskSequence;

        // Properties
        public bool AlwaysKeepTask
        {
            set { SET_CHAR_KEEP_TASK(handle, value); }
        }
        #endregion

        #region Constructor
        internal PedTaskController(IVPed targetPed)
        {
            thePed = targetPed;
            handle = targetPed.GetHandle();
            wasCreatedForTaskSequence = false;
        }
        internal PedTaskController(int pedHandle)
        {
            handle = pedHandle;
            wasCreatedForTaskSequence = true;
        }
        #endregion

        public void ClearAll()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            CLEAR_CHAR_TASKS(handle);
        }
        public void ClearAllImmediately()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            CLEAR_CHAR_TASKS_IMMEDIATELY(handle);
        }
        public void ClearSecondary()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            CLEAR_CHAR_SECONDARY_TASK(handle);
        }

        public void AchieveHeading(float heading)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_ACHIEVE_HEADING(handle, heading);
        }
        public void AimAt(Vector3 target, uint duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_AIM_GUN_AT_COORD(handle, target.X, target.Y, target.Z, duration);
        }
        public void AimAt(IVPed target, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_AIM_GUN_AT_CHAR(handle, target.GetHandle(), duration);
        }
        public void CruiseWithVehicle(IVVehicle veh, float speedMph, bool obeyTrafficLaws)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            _TASK_CAR_DRIVE_WANDER(handle, veh.GetHandle(), speedMph, (uint)(obeyTrafficLaws ? 1 : 2));
        }
        public void Die()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_DIE(handle);
        }
        public void DrivePointRoute(IVVehicle veh, float speed, Vector3[] routePoints)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;
            if (routePoints.Length == 0)
                return;

            _TASK_FLUSH_ROUTE();

            for (int i = 0; i < routePoints.Length; i++)
                _TASK_EXTEND_ROUTE(routePoints[i]);

            _TASK_DRIVE_POINT_ROUTE(handle, veh.GetHandle(), speed);
        }
        public void DriveTo(IVVehicle veh, IVPed targetPed, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null && targetPed == null))
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_PED_TARGET_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), targetPed.GetHandle(), 4, (int)speedMph, obeyTrafficLaws ? 1 : 2, 5, 10);
            else
                _TASK_CAR_MISSION_PED_TARGET(handle, veh.GetHandle(), targetPed.GetHandle(), 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void DriveTo(IVVehicle veh, IVVehicle targetVeh, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null && targetVeh == null))
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), (uint)targetVeh.GetHandle(), 1, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 10, 5);
            else
                _TASK_CAR_MISSION(handle, veh.GetHandle(), targetVeh.GetHandle(), 1, speedMph, obeyTrafficLaws ? 1 : 2, 10, 5);
        }
        public void DriveTo(IVVehicle veh, Vector3 target, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_COORS_TARGET_NOT_AGAINST_TRAFFIC(handle, veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
            else
                _TASK_CAR_MISSION_COORS_TARGET(handle, veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void EnterVehicle()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_ENTER_CAR_AS_PASSENGER(handle, 0, 0, 2);
        }
        public void EnterVehicle(IVVehicle veh, uint seat)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            if (seat == 0)
                _TASK_ENTER_CAR_AS_DRIVER(handle, veh.GetHandle(), 0);
            else
                _TASK_ENTER_CAR_AS_PASSENGER(handle, veh.GetHandle(), 0, seat);
        }
        public void FightAgainst(IVPed target, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_COMBAT_TIMED(handle, target.GetHandle(), duration);
        }
        public void FightAgainst(IVPed target)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_COMBAT(handle, target.GetHandle());
        }
        public void FightAgainstHatedTargets(float radius, uint duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR_TIMED(handle, radius, duration);
        }
        public void FightAgainstHatedTargets(float radius)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR(handle, radius);
        }
        public void FleeFromChar(IVPed target, bool onPavements, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            if (onPavements)
                _TASK_SMART_FLEE_CHAR_PREFERRING_PAVEMENTS(handle, target.GetHandle(), 100f, duration);
            else
                _TASK_SMART_FLEE_CHAR(handle, target.GetHandle(), 100f, duration);
        }
        public void FleeFromChar(IVPed target, bool onPavements)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            FleeFromChar(target, onPavements, MAX_DURATION);
        }
        public void FleeFromChar(IVPed target)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            FleeFromChar(target, false, MAX_DURATION);
        }
        public void GoTo(IVPed target, float offsetRight, float offsetFront, int duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), duration, offsetRight, offsetFront);
        }
        public void GoTo(IVPed target, float offsetRight, float offsetFront)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), MAX_DURATION, offsetRight, offsetFront);
        }
        public void GoTo(IVPed target)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_GOTO_CHAR_OFFSET(handle, target.GetHandle(), MAX_DURATION, 0f, 0f);
        }
        public void GoTo(Vector3 pos, bool ignorePaths)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(handle, pos.X, pos.Y, pos.Z, 2, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(handle, pos.X, pos.Y, pos.Z, 2, 0, 1f);
        }
        public void GoTo(Vector3 pos)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;
            
            GoTo(pos, false);
        }
        public void GuardCurrentPosition()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_GUARD_CURRENT_POSITION(handle, 15f, 10f, 1);
        }
        public void HandsUp(uint duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_HANDS_UP(handle, duration);
        }
        public void Jump(bool maybeOnSpot)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_JUMP(handle, maybeOnSpot);
        }
        public void LandHelicopter(IVVehicle veh, Vector3 pos)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            _TASK_HELI_MISSION(handle, veh.GetHandle(), 0, 0, pos.X, pos.Y, pos.Z, 5, 0f, 0, -1f, 0, 0);
        }
        public void LeaveVehicle(IVVehicle veh, bool closeDoor)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            if (closeDoor)
                _TASK_LEAVE_CAR(handle, veh.GetHandle());
            else
                _TASK_LEAVE_CAR_DONT_CLOSE_DOOR(handle, veh.GetHandle());
        }
        public void LeaveVehicle()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;
            
            _TASK_LEAVE_ANY_CAR(handle);
        }
        public void LeaveVehicleImmediately(IVVehicle veh)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            _TASK_LEAVE_CAR_IMMEDIATELY(handle, veh.GetHandle());
        }
        public void LeaveVehicleInDirection(IVVehicle veh, bool direction)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
                return;

            _TASK_LEAVE_CAR_IN_DIRECTION(handle, veh.GetHandle(), direction);
        }
        public void LookAt(Vector3 target, uint duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_LOOK_AT_COORD(handle, target.X, target.Y, target.Z, duration, 0);
        }
        public void LookAt(IVObject target, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_LOOK_AT_OBJECT(handle, target.GetHandle(), duration, 0);
        }
        public void LookAt(IVVehicle target, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_LOOK_AT_VEHICLE(handle, target.GetHandle(), duration, 0);
        }
        public void LookAt(IVPed target, uint duration)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_LOOK_AT_CHAR(handle, target.GetHandle(), duration, 0);
        }
        public void OpenDriverDoor(IVVehicle target)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_OPEN_DRIVER_DOOR(handle, target.GetHandle(), 0);
        }
        public void OpenPassengerDoor(IVVehicle target, uint seatIndex)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;
            
            _TASK_OPEN_PASSENGER_DOOR(handle, target.GetHandle(), seatIndex, 0);
        }
        public void PlayAnimation(string animSet, string animName, float speed, int unknown, AnimationFlags flags)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
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

        public void PlayAnimationSecondaryUpperBody(string animSet, string animName, float speed, int unknown1, int unknown2, int unknown3, int unknown4, int unknown5)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_PLAY_ANIM_SECONDARY_UPPER_BODY(handle, animName, animSet, speed, unknown1, unknown2, unknown3, unknown4, unknown5);
        }
        public void PlayAnimationSecondaryUpperBody(string animSet, string animName, float speed)
        {
            PlayAnimationSecondaryUpperBody(animSet, animName, speed, 0, 0, 0, 0, -1);
        }

        public void PutAwayMobilePhone()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_USE_MOBILE_PHONE(handle, false);
        }
        public void RunTo(Vector3 pos, bool ignorePaths)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(handle, pos.X, pos.Y, pos.Z, 4, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(handle, pos.X, pos.Y, pos.Z, 4, 0, 1f);
        }
        public void RunTo(Vector3 pos)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            RunTo(pos, false);
        }
        public void ShootAt(IVPed target, int duration, ShootMode mode)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_SHOOT_AT_CHAR(handle, target.GetHandle(), duration, (int)mode);
        }
        public void ShootAt(IVPed target, ShootMode mode)
        {
            ShootAt(target, -1, mode);
        }
        public void Shimmy()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_SHIMMY(handle, 1);
        }
        public bool ShimmyInDirection(bool direction)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return false;

            return _TASK_SHIMMY_IN_DIRECTION(handle, direction ? 1 : 0);
        }
        public bool ShimmyLetGo()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return false;
            
            return _TASK_SHIMMY_LET_GO(handle);
        }
        public bool ShimmyClimbUp()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return false;
            
            return _TASK_SHIMMY_CLIMB_UP(handle);
        }
        public void StandStill(int duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;
            if (duration < 0)
                duration = MAX_DURATION;

            _TASK_STAND_STILL(handle, duration);
        }
        public void SwapWeapon(eWeaponType weapon)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_SWAP_WEAPON(handle, (int)weapon);
        }
        /// <summary>
        /// Example scenario: Vehicle_LookingInBoot
        /// </summary>
        /// <param name="scenarioName">The name of the scenario to start.</param>
        /// <param name="pos">The position of the scenario to start?</param>
        public void StartScenario(string scenarioName, Vector3 pos)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_START_SCENARIO_AT_POSITION(handle, scenarioName, pos, 0);
        }
        public void TurnTo(Vector3 pos)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_TURN_CHAR_TO_FACE_COORD(handle, pos.X, pos.Y, pos.Z);
        }
        public void TurnTo(IVPed target)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && target == null))
                return;

            _TASK_TURN_CHAR_TO_FACE_CHAR(handle, target.GetHandle());
        }
        public void ToggleDuck(bool toggle)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_TOGGLE_DUCK(handle, toggle ? 1 : 0);
        }
        public void UseMobilePhone()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_USE_MOBILE_PHONE(handle, true);
        }
        public void UseMobilePhone(uint duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_USE_MOBILE_PHONE_TIMED(handle, duration);
        }
        public void Wait(int duration)
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_PAUSE(handle, duration);
        }
        public void WanderAround()
        {
            if (!wasCreatedForTaskSequence && handle == 0)
                return;

            _TASK_WANDER_STANDARD(handle);
        }
        public void WarpIntoVehicle(IVVehicle veh, uint seat)
        {
            if (!wasCreatedForTaskSequence && (handle == 0 && veh == null))
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

            sequence.Perform(thePed);
        }

    }
}

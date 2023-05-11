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

        private CPed ped;

        // Properties
        public bool AlwaysKeepTask
        {
            set {
                if (ped == null)
                    return;

                SET_CHAR_KEEP_TASK(ped.GetHandle(), value);
            }
        }
        #endregion

        #region Constructor
        internal PedTaskController(CPed targetPed)
        {
            ped = targetPed;
        }
        #endregion

        public void ClearAll()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_TASKS(ped.GetHandle());
        }
        public void ClearAllImmediately()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_TASKS_IMMEDIATELY(ped.GetHandle());
        }
        public void ClearSecondary()
        {
            if (ped == null)
                return;

            CLEAR_CHAR_SECONDARY_TASK(ped.GetHandle());
        }

        public void AimAt(Vector3 target, uint duration)
        {
            if (ped == null)
                return;

            _TASK_AIM_GUN_AT_COORD(ped.GetHandle(), target.X, target.Y, target.Z, duration);
        }
        public void AimAt(CPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_AIM_GUN_AT_CHAR(ped.GetHandle(), target.GetHandle(), duration);
        }
        public void CruiseWithVehicle(CVehicle veh, float speedMph, bool obeyTrafficLaws)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            _TASK_CAR_DRIVE_WANDER(ped.GetHandle(), veh.GetHandle(), speedMph, (uint)(obeyTrafficLaws ? 1 : 2));
        }
        public void Die()
        {
            if (ped == null)
                return;

            _TASK_DIE(ped.GetHandle());
        }
        public void DrivePointRoute(CVehicle veh, float speed, Vector3[] routePoints)
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
            _TASK_DRIVE_POINT_ROUTE(ped.GetHandle(), veh.GetHandle(), speed);
        }
        public void DriveTo(CVehicle veh, CPed targetPed, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;
            if (targetPed == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_PED_TARGET_NOT_AGAINST_TRAFFIC(ped.GetHandle(), veh.GetHandle(), targetPed.GetHandle(), 4, (int)speedMph, obeyTrafficLaws ? 1 : 2, 5, 10);
            else
                _TASK_CAR_MISSION_PED_TARGET(ped.GetHandle(), veh.GetHandle(), targetPed.GetHandle(), 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void DriveTo(CVehicle veh, CVehicle targetVeh, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;
            if (targetVeh == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_NOT_AGAINST_TRAFFIC(ped.GetHandle(), veh.GetHandle(), (uint)targetVeh.GetHandle(), 1, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 10, 5);
            else
            {
                _TASK_CAR_MISSION(ped.GetHandle(), veh.GetHandle(), (uint)targetVeh.GetHandle(), 1, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 10, 5);
            }
        }
        public void DriveTo(CVehicle veh, Vector3 target, float speedMph, bool obeyTrafficLaws, bool allowToDriveRoadsWrongWay)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (!allowToDriveRoadsWrongWay)
                _TASK_CAR_MISSION_COORS_TARGET_NOT_AGAINST_TRAFFIC(ped.GetHandle(), veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
            else
                _TASK_CAR_MISSION_COORS_TARGET(ped.GetHandle(), veh.GetHandle(), target.X, target.Y, target.Z, 4, speedMph, (uint)(obeyTrafficLaws ? 1 : 2), 5, 10);
        }
        public void EnterVehicle()
        {
            if (ped == null)
                return;

            _TASK_ENTER_CAR_AS_PASSENGER(ped.GetHandle(), 0, 0, 2);
        }
        public void EnterVehicle(CVehicle veh, uint seat)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (seat == 0)
                _TASK_ENTER_CAR_AS_DRIVER(ped.GetHandle(), veh.GetHandle(), 0);
            else
                _TASK_ENTER_CAR_AS_PASSENGER(ped.GetHandle(), veh.GetHandle(), 0, seat);
        }
        public void FightAgainst(CPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_COMBAT_TIMED(ped.GetHandle(), target.GetHandle(), duration);
        }
        public void FightAgainst(CPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_COMBAT(ped.GetHandle(), target.GetHandle());
        }
        public void FightAgainstHatedTargets(float radius, uint duration)
        {
            if (ped == null)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR_TIMED(ped.GetHandle(), radius, duration);
        }
        public void FightAgainstHatedTargets(float radius)
        {
            if (ped == null)
                return;

            _TASK_COMBAT_HATED_TARGETS_AROUND_CHAR(ped.GetHandle(), radius);
        }
        public void FleeFromChar(CPed target, bool onPavements, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            if (onPavements)
                _TASK_SMART_FLEE_CHAR_PREFERRING_PAVEMENTS(ped.GetHandle(), target.GetHandle(), 100f, duration);
            else
                _TASK_SMART_FLEE_CHAR(ped.GetHandle(), target.GetHandle(), 100f, duration);
        }
        public void FleeFromChar(CPed target, bool onPavements)
        {
            if (ped == null)
                return;

            FleeFromChar(target, onPavements, MAX_DURATION);
        }
        public void FleeFromChar(CPed target)
        {
            if (ped == null)
                return;

            FleeFromChar(target, false, MAX_DURATION);
        }
        public void GoTo(CPed target, float offsetRight, float offsetFront, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(ped.GetHandle(), target.GetHandle(), duration, offsetRight, offsetFront);
        }
        public void GoTo(CPed target, float offsetRight, float offsetFront)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(ped.GetHandle(), target.GetHandle(), MAX_DURATION, offsetRight, offsetFront);
        }
        public void GoTo(CPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_GOTO_CHAR_OFFSET(ped.GetHandle(), target.GetHandle(), MAX_DURATION, 0f, 0f);
        }
        public void GoTo(Vector3 pos, bool ignorePaths)
        {
            if (ped == null)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(ped.GetHandle(), pos.X, pos.Y, pos.Z, 2, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(ped.GetHandle(), pos.X, pos.Y, pos.Z, 2, 0, 1f);
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

            _TASK_GUARD_CURRENT_POSITION(ped.GetHandle(), 15f, 10f, 1);
        }
        public void HandsUp(uint duration)
        {
            if (ped == null)
                return;

            _TASK_HANDS_UP(ped.GetHandle(), duration);
        }
        public void LandHelicopter(CVehicle veh, Vector3 pos)
        {
            if (ped == null)
                return;

            _TASK_HELI_MISSION(ped.GetHandle(), veh.GetHandle(), 0, 0, pos.X, pos.Y, pos.Z, 5, 0f, 0, -1f, 0, 0);
        }
        public void LeaveVehicle(CVehicle veh, bool closeDoor)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (closeDoor)
                _TASK_LEAVE_CAR(ped.GetHandle(), veh.GetHandle());
            else
                _TASK_LEAVE_CAR_DONT_CLOSE_DOOR(ped.GetHandle(), veh.GetHandle());
        }
        public void LeaveVehicle()
        {
            if (ped == null)
                return;

            _TASK_LEAVE_ANY_CAR(ped.GetHandle());
        }
        public void LeaveVehicleImmediately(CVehicle veh)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            _TASK_LEAVE_CAR_IMMEDIATELY(ped.GetHandle(), veh.GetHandle());
        }
        public void LookAt(Vector3 target, uint duration)
        {
            if (ped == null)
                return;

            _TASK_LOOK_AT_COORD(ped.GetHandle(), target.X, target.Y, target.Z, duration, 0);
        }
        public void LookAt(CObject target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_OBJECT(ped.GetHandle(), target.GetHandle(), duration, 0);
        }
        public void LookAt(CVehicle target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_VEHICLE(ped.GetHandle(), target.GetHandle(), duration, 0);
        }
        public void LookAt(CPed target, uint duration)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_LOOK_AT_CHAR(ped.GetHandle(), target.GetHandle(), duration, 0);
        }
        public void PlayAnimation(string animSet, string animName, float speed, int unknown, AnimationFlags flags)
        {
            if (ped == null)
                return;

            _TASK_PLAY_ANIM_WITH_FLAGS(ped.GetHandle(), animName, animSet, speed, unknown, (int)flags);
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

            _TASK_USE_MOBILE_PHONE(ped.GetHandle(), false);
        }
        public void RunTo(Vector3 pos, bool ignorePaths)
        {
            if (ped == null)
                return;

            if (ignorePaths)
                _TASK_GO_STRAIGHT_TO_COORD(ped.GetHandle(), pos.X, pos.Y, pos.Z, 4, 45000);
            else
                _TASK_FOLLOW_NAV_MESH_TO_COORD(ped.GetHandle(), pos.X, pos.Y, pos.Z, 4, 0, 1f);
        }
        public void RunTo(Vector3 pos)
        {
            if (ped == null)
                return;

            RunTo(pos, false);
        }
        public void ShootAt(CPed target, int duration, ShootMode mode)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_SHOOT_AT_CHAR(ped.GetHandle(), target.GetHandle(), duration, (int)mode);
        }
        public void ShootAt(CPed target, ShootMode mode)
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

            _TASK_STAND_STILL(ped.GetHandle(), duration);
        }
        public void SwapWeapon(eWeaponType weapon)
        {
            if (ped == null)
                return;

            _TASK_SWAP_WEAPON(ped.GetHandle(), (uint)weapon);
        }
        // TODO: Fix native function parameters.
        //public void StartScenario(string scenarioName, Vector3 pos)
        //{
        //    if (ped == null)
        //        return;

            
        //} // "Vehicle_LookingInBoot"
        public void TurnTo(Vector3 pos)
        {
            if (ped == null)
                return;

            _TASK_TURN_CHAR_TO_FACE_COORD(ped.GetHandle(), pos.X, pos.Y, pos.Z);
        }
        public void TurnTo(CPed target)
        {
            if (ped == null)
                return;
            if (target == null)
                return;

            _TASK_TURN_CHAR_TO_FACE_CHAR(ped.GetHandle(), target.GetHandle());
        }
        public void UseMobilePhone()
        {
            if (ped == null)
                return;

            _TASK_USE_MOBILE_PHONE(ped.GetHandle(), true);
        }
        public void UseMobilePhone(uint duration)
        {
            if (ped == null)
                return;

            _TASK_USE_MOBILE_PHONE_TIMED(ped.GetHandle(), duration);
        }
        public void Wait(uint duration)
        {
            if (ped == null)
                return;

            _TASK_PAUSE(ped.GetHandle(), duration);
        }
        public void WanderAround()
        {
            if (ped == null)
                return;

            _TASK_WANDER_STANDARD(ped.GetHandle());
        }
        public void WarpIntoVehicle(CVehicle veh, uint seat)
        {
            if (ped == null)
                return;
            if (veh == null)
                return;

            if (seat == 0)
                _TASK_WARP_CHAR_INTO_CAR_AS_DRIVER(ped.GetHandle(), veh.GetHandle());
            else
                _TASK_WARP_CHAR_INTO_CAR_AS_PASSENGER(ped.GetHandle(), veh.GetHandle(), seat);
        }

    }
}

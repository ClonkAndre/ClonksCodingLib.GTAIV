using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve peds.
    /// </summary>
    public class NativePed : HandleObject
    {

        #region Properties

        public Vector3 Position
        {
            get
            {
                if (!Exists())
                    return Vector3.Zero;

                GET_CHAR_COORDINATES(Handle, out Vector3 pos);
                return pos;
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_COORDINATES(Handle, value);
            }
        }
        public float Heading
        {
            get
            {
                if (!Exists())
                    return 0f;

                GET_CHAR_HEADING(Handle, out float heading);
                return heading;
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_HEADING(Handle, value);
            }
        }
        public Vector3 Direction
        {
            get
            {
                if (!Exists())
                    return Vector3.Zero;

                return Helper.HeadingToDirection(Heading);
            }
        }
        public Vector3 Velocity
        {
            get
            {
                if (!Exists())
                    return Vector3.Zero;

                GET_CHAR_VELOCITY(Handle, out Vector3 vel);
                return vel;
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_VELOCITY(Handle, value.X, value.Y, value.Z);
            }
        }

        public NativeRoom CurrentRoom
        {
            get
            {
                if (!Exists())
                    return null;

                return NativeRoom.FromPed(Handle);
            }
            set
            {
                if (!Exists())
                    return;
                if (value == null)
                    return;

                SET_ROOM_FOR_CHAR_BY_KEY(Handle, (uint)value.Room);
            }
        }
        public NativeModel Model
        {
            get
            {
                if (!Exists())
                    return NativeModel.Empty();

                GET_CHAR_MODEL(Handle, out int model);
                return new NativeModel(model);
            }
        }
        
        public PedGender Gender
        {
            get
            {
                if (!Exists())
                    return PedGender.Unknown;

                return IS_CHAR_MALE(Handle) ? PedGender.Male : PedGender.Female;
            }
        }
        public ePedType PedType
        {
            get
            {
                if (!Exists())
                    return ePedType.PED_TYPE_CIV_MALE;

                GET_PED_TYPE(Handle, out uint type);
                return (ePedType)type;
            }
        }
        public eRelationshipGroup RelationshipGroup
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_RELATIONSHIP_GROUP(Handle, (int)value);
            }
        }

        public PedTaskController Task
        {
            get
            {
                if (!Exists())
                    return PedTaskController.Empty();

                return PedTaskController.FromHandle(Handle);
            }
        }
        public PedAnimationController Animation
        {
            get
            {
                if (!Exists())
                    return PedAnimationController.Empty();

                return new PedAnimationController(Handle);
            }
        }

        public int CurrentVehicle
        {
            get
            {
                if (!Exists())
                    return 0;

                GET_CAR_CHAR_IS_USING(Handle, out int v);
                return v;
            }
        }

        public bool Visible
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_VISIBLE(Handle, value);
            }
        }
        public bool IsAlive
        {
            get
            {
                if (!Exists())
                    return false;

                return !IsDead;
            }
        }
        public bool IsAliveAndWell
        {
            get
            {
                if (!Exists())
                    return false;

                return IsAlive && !IsInjured;
            }
        }
        public bool IsDead
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_DEAD(Handle);
            }
        }
        public bool IsGettingIntoAVehicle
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_GETTING_IN_TO_A_CAR(Handle);
            }
        }
        public bool IsGettingUp
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_GETTING_UP(Handle);
            }
        }
        public bool IsOnFire
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_ON_FIRE(Handle);
            }
        }
        public bool IsIdle
        {
            get
            {
                if (!Exists())
                    return false;

                if (IS_CHAR_INJURED(Handle))
                    return false;
                if (IS_PED_RAGDOLL(Handle))
                    return false;
                if (IS_CHAR_IN_AIR(Handle))
                    return false;
                if (IS_CHAR_ON_FIRE(Handle))
                    return false;
                if (IS_CHAR_DUCKING(Handle))
                    return false;
                if (IS_CHAR_GESTURING(Handle))
                    return false;
                if (IS_CHAR_GETTING_IN_TO_A_CAR(Handle))
                    return false;
                if (IS_AMBIENT_SPEECH_PLAYING(Handle))
                    return false;
                if (IS_SCRIPTED_SPEECH_PLAYING(Handle))
                    return false;
                if (IS_CHAR_IN_MELEE_COMBAT(Handle))
                    return false;
                if (IS_PED_IN_COMBAT(Handle))
                    return false;
                if ((IS_CHAR_IN_ANY_CAR(Handle)) && (!IS_CHAR_SITTING_IN_ANY_CAR(Handle)))
                    return false;

                return true;
            }
        }
        public bool IsInAir
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_IN_AIR(Handle);
            }
        }
        public bool IsInCombat
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_PED_IN_COMBAT(Handle);
            }
        }
        public bool IsInGroup
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_PED_IN_GROUP(Handle);
            }
        }
        public bool IsInMeleeCombat
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_IN_MELEE_COMBAT(Handle);
            }
        }
        public bool IsInjured
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_INJURED(Handle);
            }
        }
        public bool IsInWater
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_IN_WATER(Handle);
            }
        }
        public bool IsOnScreen
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_ON_SCREEN(Handle);
            }
        }
        public bool IsRagdoll
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_PED_RAGDOLL(Handle);
            }
            set
            {
                if (!Exists())
                    return;

                if (value)
                {
                    PreventRagdoll = false;
                    SWITCH_PED_TO_RAGDOLL(Handle, 10000, -1, false, true, true, false);
                }
                else
                {
                    SWITCH_PED_TO_ANIMATED(Handle, false);
                }
            }
        }
        public bool IsShooting
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_SHOOTING(Handle);
            }
        }
        public bool IsSwimming
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_CHAR_SWIMMING(Handle);
            }
        }
        public bool IsRequiredForMission
        {
            get
            {
                if (!Exists())
                    return false;

                return IS_PED_A_MISSION_PED(Handle);
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_AS_MISSION_CHAR(Handle);
            }
        }
        public bool FreezePosition
        {
            set
            {
                if (!Exists())
                    return;

                FREEZE_CHAR_POSITION(Handle, value);
            }
        }

        public string Voice
        {
            set
            {
                if (!Exists())
                    return;

                if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "default")
                {
                    SetDefaultVoice();
                    return;
                }

                SET_AMBIENT_VOICE_NAME(Handle, value);
            }
        }

        public int Health
        {
            get
            {
                if (!Exists())
                    return 0;

                GET_CHAR_HEALTH(Handle, out uint health);
                return (int)health;
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_HEALTH(Handle, (uint)value);
            }
        }
        public int MaxHealth
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_MAX_HEALTH(Handle, (uint)value);
            }
        }
        public int Armour
        {
            get
            {
                if (!Exists())
                    return 0;

                GET_CHAR_ARMOUR(Handle, out uint v);
                return (int)v;
            }
            set
            {
                if (!Exists())
                    return;

                int diff = value - Armour;

                if (diff == 0)
                    return;

                ADD_ARMOUR_TO_CHAR(Handle, diff);
            }
        }
        public int Money
        {
            get
            {
                if (!Exists())
                    return 0;

                return (int)GET_CHAR_MONEY(Handle);
            }
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_MONEY(Handle, (uint)value);
            }
        }
        public int Accuracy
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_ACCURACY(Handle, (uint)value);
            }
        }

        public bool AlwaysDiesOnLowHealth
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WILL_MOVE_WHEN_INJURED(Handle, !value);
                SET_PED_DIES_WHEN_INJURED(Handle, value);
            }
        }
        public bool BlockPermanentEvents
        {
            set
            {
                if (!Exists())
                    return;

                SET_BLOCKING_OF_NON_TEMPORARY_EVENTS(Handle, value);
            }
        }
        public bool BlockWeaponSwitching
        {
            set
            {
                if (!Exists())
                    return;

                BLOCK_PED_WEAPON_SWITCHING(Handle, value);
            }
        }
        public bool BlockGestures
        {
            set
            {
                if (!Exists())
                    return;

                BLOCK_CHAR_GESTURE_ANIMS(Handle, value);
            }
        }
        public bool CanBeDraggedOutOfVehicle
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_CANT_BE_DRAGGED_OUT(Handle, !value);
            }
        }
        public bool CanBeKnockedOffBike
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_CAN_BE_KNOCKED_OFF_BIKE(Handle, value);
            }
        }
        public bool CanSwitchWeapons
        {
            set
            {
                if (!Exists())
                    return;

                BLOCK_PED_WEAPON_SWITCHING(Handle, !value);
            }
        }
        public bool CowerInsteadOfFleeing
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WILL_COWER_INSTEAD_OF_FLEEING(Handle, value);
            }
        }
        public bool DuckWhenAimedAtByGroupMember
        {
            set
            {
                if (!Exists())
                    return;

                SET_GROUP_CHAR_DUCKS_WHEN_AIMED_AT(Handle, value);
            }
        }
        public bool Enemy
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_AS_ENEMY(Handle, value);
            }
        }
        public bool Invincible
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_INVINCIBLE(Handle, value);
            }
        }
        public bool PreventRagdoll
        {
            set
            {
                if (!Exists())
                    return;

                UNLOCK_RAGDOLL(Handle, !value);
            }
        }
        public bool PriorityTargetForEnemies
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_IS_TARGET_PRIORITY(Handle, value);
            }
        }
        public bool WantedByPolice
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WANTED_BY_POLICE(Handle, value);
            }
        }
        public bool WillDoDrivebys
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WILL_DO_DRIVEBYS(Handle, value);
            }
        }
        public bool WillFlyThroughWindscreen
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WILL_FLY_THROUGH_WINDSCREEN(Handle, value);
            }
        }
        public bool WillUseCarsInCombat
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_WILL_USE_CARS_IN_COMBAT(Handle, value);
            }
        }

        public float HeightAboveGround
        {
            get
            {
                if (!Exists())
                    return 0f;

                GET_CHAR_HEIGHT_ABOVE_GROUND(Handle, out float f);
                return f;
            }
        }
        public float FireDamageMultiplier
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_FIRE_DAMAGE_MULTIPLIER(Handle, value);
            }
        }
        public float GravityMultiplier
        {
            set
            {
                if (!Exists())
                    return;

                SET_CHAR_GRAVITY(Handle, value);
            }
        }
        public float SenseRange
        {
            set
            {
                if (!Exists())
                    return;

                SET_SENSE_RANGE(Handle, value);
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativePed"/> class with an existing handle.
        /// </summary>
        /// <param name="handle">The handle of an already existing ped.</param>
        public NativePed(int handle) : base(handle)
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
                DELETE_CHAR(ref handle);
            }

            base.Delete();
        }

        public void ApplyForce(Vector3 direction, Vector3 rotation)
        {
            if (!Exists())
                return;

            APPLY_FORCE_TO_PED(Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public void ApplyForce(Vector3 direction)
        {
            ApplyForce(direction, Vector3.Zero);
        }
        public void ApplyForceRelative(Vector3 direction, Vector3 rotation)
        {
            if (!Exists())
                return;

            APPLY_FORCE_TO_PED(Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public void ApplyForceRelative(Vector3 direction)
        {
            ApplyForceRelative(direction, Vector3.Zero);
        }

        public void AttachTo(int vehicle, Vector3 offset)
        {
            if (!Exists())
                return;
            if (vehicle == 0)
                return;

            ATTACH_PED_TO_CAR(Handle, vehicle, 0, offset.X, offset.Y, offset.Z, 0.0f, 0.0f, 0, false);
        }
        public void Detach()
        {
            if (!Exists())
                return;

            DETACH_PED(Handle, true);
        }

        public void ChangeRelationship(eRelationshipGroup group, eRelationship level)
        {
            if (!Exists())
                return;

            SET_CHAR_RELATIONSHIP(Handle, (uint)level, (int)group);
        }
        public void CantBeDamagedByRelationshipGroup(eRelationshipGroup group, bool value)
        {
            if (!Exists())
                return;

            SET_CHAR_NOT_DAMAGED_BY_RELATIONSHIP_GROUP(Handle, (int)group, value);
        }

        public void SetPathfinding(bool allowClimbovers, bool allowLadders, bool allowDropFromHeight)
        {
            if (!Exists())
                return;

            SET_PED_PATH_MAY_USE_CLIMBOVERS(Handle, allowClimbovers);
            SET_PED_PATH_MAY_USE_LADDERS(Handle, allowLadders);
            SET_PED_PATH_MAY_DROP_FROM_HEIGHT(Handle, allowDropFromHeight);
        }
        public void SetDefaultVoice()
        {
            if (!Exists())
                return;

            SET_VOICE_ID_FROM_HEAD_COMPONENT(Handle, 0, IS_CHAR_MALE(Handle));
        }

        public void GiveFakeNetworkName(string name, Color Color)
        {
            if (!Exists())
                return;

            GIVE_PED_FAKE_NETWORK_NAME(Handle, name, Color.R, Color.G, Color.B, Color.A);
        }
        public void RemoveFakeNetworkName()
        {
            if (!Exists())
                return;

            REMOVE_FAKE_NETWORK_NAME_FROM_PED(Handle);
        }

        public void FleeByVehicle(int vehicle)
        {
            if (!Exists())
                return;
            if (vehicle == 0)
                return;

            FORCE_PED_TO_FLEE_WHILST_DRIVING_VEHICLE(Handle, vehicle);
        }

        public void ForceHelmet(bool enable)
        {
            if (!Exists())
                return;

            if (enable)
                GIVE_PED_HELMET(Handle);
            else
                REMOVE_PED_HELMET(Handle, true);
        }
        public void ForceRagdoll(int duration, bool tryToStayUpright)
        {
            if (!Exists())
                return;

            SWITCH_PED_TO_RAGDOLL(Handle, 10000, duration, (tryToStayUpright ? true : false), true, true, false);
        }

        public void SetDefensiveArea(Vector3 position, float radius)
        {
            if (!Exists())
                return;

            SET_CHAR_SPHERE_DEFENSIVE_AREA(Handle, position.X, position.Y, position.Z, radius);
        }
        public void MakeProofTo(bool bullets, bool fire, bool explosions, bool fallingDamage, bool meleeAttacks)
        {
            if (!Exists())
                return;

            SET_CHAR_PROOFS(Handle, bullets, fire, explosions, fallingDamage, meleeAttacks);
        }

        public void ShootAt(Vector3 position)
        {
            if (!Exists())
                return;

            FIRE_PED_WEAPON(Handle, position.X, position.Y, position.Z);
        }

        public void WarpIntoVehicle(int vehicle, VehicleSeat seat)
        {
            if (!Exists())
                return;
            if (vehicle == 0)
                return;

            if (seat <= VehicleSeat.None)
                return;
            if (seat == VehicleSeat.Driver)
            {
                WARP_CHAR_INTO_CAR(Handle, vehicle);
            }
            else
            {
                if (IsInVehicle(vehicle))
                    WARP_CHAR_FROM_CAR_TO_CAR(Handle, vehicle, (uint)seat); // change seat
                else
                    WARP_CHAR_INTO_CAR_AS_PASSENGER(Handle, vehicle, (uint)seat);
            }
        }

        public void DropCurrentWeapon()
        {
            if (!Exists())
                return;

            FORCE_CHAR_TO_DROP_WEAPON(Handle);
        }
        public void Die()
        {
            if (!Exists())
                return;

            _TASK_DIE(Handle);
        }
        public void LeaveGroup()
        {
            if (!Exists())
                return;

            REMOVE_CHAR_FROM_GROUP(Handle);
        }
        public void LeaveVehicle()
        {
            if (!Exists())
                return;

            _TASK_LEAVE_ANY_CAR(Handle);
        }
        public void RandomizeOutfit()
        {
            if (!Exists())
                return;

            SET_CHAR_RANDOM_COMPONENT_VARIATION(Handle);
        }
        public void BecomeMissionCharacter()
        {
            if (!Exists())
                return;

            SET_CHAR_AS_MISSION_CHAR(Handle);
        }
        public void NoLongerNeeded()
        {
            if (!Exists())
                return;

            MARK_CHAR_AS_NO_LONGER_NEEDED(Handle);
        }

        public void CancelAmbientSpeech()
        {
            if (!Exists())
                return;

            CANCEL_CURRENTLY_PLAYING_AMBIENT_SPEECH(Handle);
        }
        public void SayAmbientSpeech(string phraseID)
        {
            if (!Exists())
                return;

            CancelAmbientSpeech();
            SAY_AMBIENT_SPEECH(Handle, phraseID, true, true, 0);
        }

        public void StartKillingSpree(bool alsoAttackPlayer)
        {
            if (!Exists())
                return;

            int i = 1;
            if (alsoAttackPlayer)
                i = 0;

            for (; i <= 22; i++)
                ChangeRelationship((eRelationshipGroup)i, eRelationship.RELATIONSHIP_HATE);

            AlwaysDiesOnLowHealth = true;

            if (IsInVehicle())
            {
                WillUseCarsInCombat = true;
                WillDoDrivebys = true;
            }

            Task.ClearAll();
            Task.SetAlwaysKeepTask(true);
            Task.FightAgainstHatedTargets(20.0F);
        }

        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_CHAR_EXIST(Handle);
        }

        public NativeBlip AttachBlip()
        {
            if (!Exists())
                return null;

            return NativeBlip.AddBlip(Handle);
        }

        public bool IsAttachedToVehicle()
        {
            if (!Exists())
                return false;

            return IS_PED_ATTACHED_TO_ANY_CAR(Handle);
        }
        public bool HasBeenDamagedBy(eWeaponType weapon)
        {
            if (!Exists())
                return false;

            return HAS_CHAR_BEEN_DAMAGED_BY_WEAPON(Handle, (int)weapon);
        }
        public bool HasBeenDamagedBy(int vehicle) // TODO: Add NativeVehicle class
        {
            if (!Exists())
                return false;

            return HAS_CHAR_BEEN_DAMAGED_BY_CAR(Handle, vehicle);
        }
        public bool HasBeenDamagedBy(NativePed ped)
        {
            if (ped == null)
                return false;
            if (!Exists())
                return false;
            if (!ped.Exists())
                return false;

            return HAS_CHAR_BEEN_DAMAGED_BY_CHAR(Handle, ped.Handle, false);
        }

        public bool IsInVehicle(int vehicle)
        {
            if (!Exists())
                return false;

            return IS_CHAR_IN_CAR(Handle, vehicle);
        }
        public bool IsInVehicle()
        {
            if (!Exists())
                return false;

            return IS_CHAR_IN_ANY_CAR(Handle);
        }
        public bool IsSittingInVehicle(int vehicle)
        {
            if (!Exists())
                return false;

            return IS_CHAR_SITTING_IN_CAR(Handle, vehicle);
        }
        public bool IsSittingInVehicle()
        {
            if (!Exists())
                return false;

            return IS_CHAR_SITTING_IN_ANY_CAR(Handle);
        }

        //public bool IsTouching() // TODO: Add NativeObject class
        //{
        //    if (!Exists())
        //        return false;


        //}
        public bool IsTouching(int vehicle)
        {
            if (!Exists())
                return false;

            return IS_CHAR_TOUCHING_VEHICLE(Handle, vehicle);
        }
        public bool IsTouching(NativePed ped)
        {
            if (ped == null)
                return false;
            if (!Exists())
                return false;
            if (!ped.Exists())
                return false;

            return IS_CHAR_TOUCHING_CHAR(Handle, ped.Handle);
        }
        public bool IsInArea(Vector3 corner1, Vector3 corner2, bool ignoreHeight)
        {
            if (!Exists())
                return false;

            if (ignoreHeight)
                return IS_CHAR_IN_AREA_2D(Handle, corner1.X, corner1.Y, corner2.X, corner2.Y, false);
            else
                return IS_CHAR_IN_AREA_3D(Handle, corner1.X, corner1.Y, corner1.Z, corner2.X, corner2.Y, corner2.Z, false);
        }

        public Vector3 GetBonePosition(eBone bone)
        {
            if (!Exists())
                return Vector3.Zero;

            GET_PED_BONE_POSITION(Handle, (uint)bone, Vector3.Zero, out Vector3 pos);
            return pos;
        }
        public Vector3 GetOffsetPosition(Vector3 offset)
        {
            if (!Exists())
                return Vector3.Zero;

            GET_OFFSET_FROM_CHAR_IN_WORLD_COORDS(Handle, offset, out Vector3 o);
            return o;
        }

        //public NativePlayer GetControllingPlayer() // TODO: Add NativePlayer class
        //{
        //    if (!Exists())
        //        return null;


        //}

        #endregion

    }
}

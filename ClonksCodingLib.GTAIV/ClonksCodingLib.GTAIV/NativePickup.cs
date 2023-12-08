using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativePickup : HandleObject
    {

        // ADD_BLIP_FOR_PICKUP
        // ADD_PICKUP_TO_INTERIOR_ROOM_BY_KEY
        // ADD_PICKUP_TO_INTERIOR_ROOM_BY_NAME

        // GET_BLIP_INFO_ID_PICKUP_INDEX(?)
        // REMOVE_TEMPORARY_RADAR_BLIPS_FOR_PICKUPS
        // SWITCH_ARROW_ABOVE_BLIPPED_PICKUPS
        // CREATE_TEMPORARY_RADAR_BLIPS_FOR_PICKUPS_IN_AREA
        // DISABLE_LOCAL_PLAYER_PICKUPS
        // CHANGE_PICKUP_BLIP_SCALE(float scale);
        // CHANGE_PICKUP_BLIP_PRIORITY(int priority);
        // CHANGE_PICKUP_BLIP_DISPLAY(int display);
        // CHANGE_PICKUP_BLIP_SPRITE(int sprite);
        // CHANGE_PICKUP_BLIP_COLOUR(int colour)
        // GIVE_PED_PICKUP_OBJECT

        #region Properties
        /// <summary>
        /// Gets the position of this pickup.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (!IsValid)
                    return Vector3.Zero;

                GET_PICKUP_COORDINATES(Handle, out float x, out float y, out float z);
                return new Vector3(x, y, z);
            }
        }
        /// <summary>
        /// Gets if this pickup has been collected.
        /// </summary>
        public bool HasBeenCollected
        {
            get
            {
                if (!IsValid)
                    return false;

                return HAS_PICKUP_BEEN_COLLECTED(Handle);
            }
        }
        /// <summary>
        /// Sets if parked cars can spawn on top of this pickup.
        /// </summary>
        public bool DoNotSpawnParkedCarsOnTop
        {
            set
            {
                if (!IsValid)
                    return;

                SET_DO_NOT_SPAWN_PARKED_CARS_ON_TOP(Handle, value);
            }
        }
        /// <summary>
        /// Sets if this pickup can be collected by car.
        /// </summary>
        public bool CollectableByCar
        {
            set
            {
                if (!IsValid)
                    return;

                SET_PICKUP_COLLECTABLE_BY_CAR(Handle, value);
            }
        }
        /// <summary>
        /// Gets the current room hash of the pickup.
        /// </summary>
        public uint RoomHash
        {
            get
            {
                if (!IsValid)
                    return 0;

                GET_ROOM_KEY_FROM_PICKUP(Handle, out uint hash);
                return hash;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativePickup"/> class with an existing handle.
        /// </summary>
        /// <param name="handle">The handle of an already existing pickup.</param>
        public NativePickup(int handle) : base(handle)
        {

        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Dispose()
        {
            if (Exists())
                REMOVE_PICKUP(Handle);

            base.Dispose();
        }

        /// <summary>
        /// Adds a simple blip for this pickup.
        /// </summary>
        public void AddSimpleBlip()
        {
            if (!IsValid)
                return;
            
            ADD_SIMPLE_BLIP_FOR_PICKUP(Handle);
        }

        // Statics
        /// <summary>
        /// Creates a pickup at the given location.
        /// </summary>
        /// <param name="model">The pickup model.</param>
        /// <param name="type">The pickup type.</param>
        /// <param name="pos">The pickup location.</param>
        /// <returns>If successful, the newly created pickup is returned. Otherwise, false.</returns>
        public static NativePickup CreatePickup(ePickupModel model, ePickupType type, Vector3 pos)
        {
            CREATE_PICKUP((uint)model, (uint)type, pos.X, pos.Y, pos.Z, out int handle, false);

            if (handle == 0)
                return null;

            return new NativePickup(handle);
        }

        /// <summary>
        /// Creates a pickup at the given location and rotation.
        /// </summary>
        /// <param name="model">The pickup model.</param>
        /// <param name="type">The pickup type.</param>
        /// <param name="pos">The pickup location.</param>
        /// <param name="rot">The pickup rotation.</param>
        /// <returns>If successful, the newly created pickup is returned. Otherwise, false.</returns>
        public static NativePickup CreatePickup(ePickupModel model, ePickupType type, Vector3 pos, Vector3 rot)
        {
            CREATE_PICKUP_ROTATE((uint)model, (uint)type, 0, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, out int handle);

            if (handle == 0)
                return null;

            return new NativePickup(handle);
        }

        /// <summary>
        /// Creates a weapon pickup at the given location with the given ammo.
        /// </summary>
        /// <param name="model">The pickup model.</param>
        /// <param name="ammo">The pickup ammo amount.</param>
        /// <param name="type">The pickup type.</param>
        /// <param name="pos">The pickup location.</param>
        /// <returns>If successful, the newly created pickup is returned. Otherwise, false.</returns>
        public static NativePickup CreateWeaponPickup(ePickupModel model, ePickupType type, uint ammo, Vector3 pos)
        {
            CREATE_PICKUP_WITH_AMMO((uint)model, (uint)type, ammo, pos.X, pos.Y, pos.Z, out int handle);

            if (handle == 0)
                return null;

            return new NativePickup(handle);
        }

        /// <summary>
        /// Creates a money pickup at the given location with the given amount.
        /// </summary>
        /// <param name="pos">The pickup position.</param>
        /// <param name="amount">The money amount.</param>
        /// <returns>If successful, the newly created pickup is returned. Otherwise, false.</returns>
        public static NativePickup CreateMoneyPickup(Vector3 pos, uint amount)
        {
            CREATE_MONEY_PICKUP(pos.X, pos.Y, pos.Z, amount, true, out int handle);

            if (handle == 0)
                return null;

            return new NativePickup(handle);
        }

        /// <summary>
        /// Sets if weapon pickups should be rendered bigger in-game.
        /// </summary>
        /// <param name="value">True, weapon pickups will render bigger. False, weapon pickups will not render bigger.</param>
        public static void RenderWeaponPickupsBigger(bool value)
        {
            RENDER_WEAPON_PICKUPS_BIGGER(value);
        }

        /// <summary>
        /// Sets if pickups of the given type can be collected by car.
        /// </summary>
        /// <param name="type">The pickup type this should apply on.</param>
        /// <param name="value">True if pickups of given type will be collectable by car. False if pickups of given type will not be collectable by car.</param>
        public static void SetAllPickupsOfTypeCollectableByCar(ePickupType type, bool value)
        {
            SET_ALL_PICKUPS_OF_TYPE_COLLECTABLE_BY_CAR((int)type, value);
        }

        /// <summary>
        /// Probably sets the message that will be displayed when the game asks you to replace the current weapon with the new weapon.
        /// </summary>
        /// <param name="value">True if the message should always display. Otherwise, false.</param>
        public static void SetAlwaysDisplayWeaponPickupMessage(bool value)
        {
            SET_ALWAYS_DISPLAY_WEAPON_PICKUP_MESSAGE(value);
        }

        /// <summary>
        /// Removes all pickups of the given type.
        /// </summary>
        /// <param name="type">The target type.</param>
        public static void RemoveAllPickupsOfType(ePickupType type)
        {
            REMOVE_ALL_PICKUPS_OF_TYPE((uint)type);
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_PICKUP_EXIST(Handle);
        }

        /// <summary>
        /// Gets if the pickup has been collected by the given player.
        /// </summary>
        /// <param name="playerIndex">The player to check on.</param>
        /// <returns>True if the given player has collected the pickup. Otherwise, false.</returns>
        public bool HasPlayerCollectedPickup(int playerIndex)
        {
            if (!IsValid)
                return false;

            return HAS_PLAYER_COLLECTED_PICKUP(playerIndex, Handle);
        }

        // Statics
        /// <summary>
        /// Gets a safe position for a pickup from the given position.
        /// </summary>
        /// <param name="targetPos">Target position to get a safe pickup position from.</param>
        /// <returns>The safe position.</returns>
        public static Vector3 GetSafePickupPositionAtPos(Vector3 targetPos)
        {
            GET_SAFE_PICKUP_COORDS(targetPos.X, targetPos.Y, targetPos.Z, out float x, out float y, out float z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Gets if any pickup is at the given position.
        /// </summary>
        /// <param name="pos">Target position to check for.</param>
        /// <returns>True if there is any pickup at the position. Otherwise, false.</returns>
        public static bool IsAnyPickupAtPos(Vector3 pos)
        {
            return IS_ANY_PICKUP_AT_COORDS(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Gets if any money pickup is at the given position.
        /// </summary>
        /// <param name="pos">Target position to check for.</param>
        /// <returns>True if there is any money pickup at the position. Otherwise, false.</returns>
        public static bool IsMoneyPickupAtPos(Vector3 pos)
        {
            return IS_MONEY_PICKUP_AT_COORDS(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Sets if pickups fix cars.
        /// </summary>
        /// <param name="value">True, pickups will fix cars. False, pickups will not fix cars.</param>
        public static void SetPickupsFixCars(bool value)
        {
            SET_PICKUPS_FIX_CARS(value);
        }

        /// <summary>
        /// Attaches a <see cref="NativeBlip"/> to this <see cref="NativePickup"/>.
        /// </summary>
        /// <returns>If successful, the attached <see cref="NativeBlip"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public NativeBlip AttachBlip()
        {
            if (!IsValid)
                return null;
            if (!Exists())
                return null;

            return NativeBlip.AddBlip(this);
        }
        #endregion

    }
}

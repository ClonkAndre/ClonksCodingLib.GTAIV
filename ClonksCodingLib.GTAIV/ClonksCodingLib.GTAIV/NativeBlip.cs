using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativeBlip : HandleObject
    {
        #region Properties
        public Vector3 Position
        {
            get {
                if (!IsValid)
                    return Vector3.Zero;

                GET_BLIP_COORDS(Handle, out Vector3 pos);
                return pos;
            }
        }

        public eBlipColor Color
        {
            get {
                if (!IsValid)
                    return eBlipColor.BLIP_COLOR_WHITE;

                GET_BLIP_COLOUR(Handle, out uint c);
                return (eBlipColor)c;
            }
            set {
                if (IsValid)
                    CHANGE_BLIP_COLOUR(Handle, (int)value);
            }
        }
        public eBlipDisplay Display
        {
            get {
                if (!IsValid)
                    return eBlipDisplay.BLIP_DISPLAY_HIDDEN;

                return (eBlipDisplay)GET_BLIP_INFO_ID_DISPLAY(Handle);
            }
            set {
                if (IsValid)
                    CHANGE_BLIP_DISPLAY(Handle, (uint)value);
            }
        }
        public BlipIcon Icon
        {
            get {
                if (!IsValid)
                    return BlipIcon.Misc_Destination;

                return (BlipIcon)GET_BLIP_SPRITE(Handle);
            }
            set {
                if (IsValid)
                    CHANGE_BLIP_SPRITE(Handle, (uint)value);
            }
        }
        public eBlipType Type
        {
            get {
                if (!IsValid)
                    return eBlipType.BLIP_TYPE_UNKNOWN;

                return (eBlipType)GET_BLIP_INFO_ID_TYPE(Handle);
            }
        }
        public uint Priority
        {
            set {
                if (IsValid)
                    CHANGE_BLIP_PRIORITY(Handle, value);
            }
        }
        public bool Friendly
        {
            set {
                if (IsValid)
                    SET_BLIP_AS_FRIENDLY(Handle, value);
            }
        }
        public bool ShowOnlyWhenNear
        {
            get {
                if (!IsValid)
                    return false;

                return IS_BLIP_SHORT_RANGE(Handle);
            }
            set {
                if (IsValid)
                    SET_BLIP_AS_SHORT_RANGE(Handle, value);
            }
        }
        public bool RouteActive
        {
            set {
                if (IsValid)
                    SET_ROUTE(Handle, value);
            }
        }
        public bool FlashBlip
        {
            set {
                if (IsValid)
                    FLASH_BLIP(Handle, value);
            }
        }
        public bool FlashBlip2
        {
            set {
                if (IsValid)
                    FLASH_BLIP_ALT(Handle, value);
            }
        }
        public float Scale
        {
            set {
                if (IsValid)
                    CHANGE_BLIP_SCALE(Handle, value);
            }
        }
        public int Transparency
        {
            set {
                if (IsValid)
                    CHANGE_BLIP_ALPHA(Handle, value);
            }
        }
        
        public string Name
        {
            set {
                if (IsValid)
                    CHANGE_BLIP_NAME_FROM_ASCII(Handle, value);
            }
        }
        #endregion

        #region Constructor
        internal NativeBlip(int handle) : base(handle)
        {

        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Dispose()
        {
            if (Exists())
                REMOVE_BLIP(Handle);
            
            base.Dispose();
        }

        public void SetColorRGB(Color color)
        {
            if (IsValid)
                CHANGE_BLIP_COLOUR(Handle, color.ToRgba());
        }
        public void SetTerritoryBlipScale(SizeF scale)
        {
            if (IsValid)
                CHANGE_TERRITORY_BLIP_SCALE(Handle, scale.Width, scale.Height);
        }

        // Statics
        public static void AddSimpleBlip(NativePickup target)
        {
            if (target == null)
                return;

            ADD_SIMPLE_BLIP_FOR_PICKUP(target.Handle);
        }
        public static void RemoveTemporaryPickupBlips()
        {
            REMOVE_TEMPORARY_RADAR_BLIPS_FOR_PICKUPS();
        }
        public static void SwitchArrowAboveBlippedPickups(bool on)
        {
            SWITCH_ARROW_ABOVE_BLIPPED_PICKUPS(on);
        }
        public static void SetBlipThrottleRandomly(CVehicle veh, bool on)
        {
            if (veh == null)
                return;

            SET_BLIP_THROTTLE_RANDOMLY(veh.GetHandle(), on);
        }
        public static void SetPoliceRadarBlips(bool on)
        {
            SET_POLICE_RADAR_BLIPS(on);
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_BLIP_EXIST(Handle);
        }

        /// <summary>
        /// Gets the handle of the object this <see cref="NativeBlip"/> is attached to.
        /// </summary>
        /// <returns>The handle this <see cref="NativeBlip"/> is attached to..</returns>
        public int GetAttachedItemHandle()
        {
            if (!IsValid || !Exists())
                return 0;

            switch (Type)
            {
                case eBlipType.BLIP_TYPE_CAR:       return GET_BLIP_INFO_ID_CAR_INDEX(Handle);
                case eBlipType.BLIP_TYPE_CHAR:      return GET_BLIP_INFO_ID_PED_INDEX(Handle);
                case eBlipType.BLIP_TYPE_OBJECT:    return GET_BLIP_INFO_ID_OBJECT_INDEX(Handle);
                case eBlipType.BLIP_TYPE_PICKUP:    return GET_BLIP_INFO_ID_PICKUP_INDEX(Handle);
            }

            return 0;
        }

        // Statics
        public static NativeBlip AddBlip(NativePickup target)
        {
            if (target == null)
                return null;

            ADD_BLIP_FOR_PICKUP(target.Handle, out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        public static NativeBlip AddBlip(Vector3 target)
        {
            ADD_BLIP_FOR_COORD(target.X, target.Y, target.Z, out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        public static NativeBlip AddBlip(CVehicle target)
        {
            if (target == null)
                return null;

            ADD_BLIP_FOR_CAR(target.GetHandle(), out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        public static NativeBlip AddBlip(CPed target)
        {
            if (target == null)
                return null;

            ADD_BLIP_FOR_CHAR(target.GetHandle(), out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        public static NativeBlip AddBlipContact(Vector3 target)
        {
            ADD_BLIP_FOR_CONTACT(target.X, target.Y, target.Z, out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        public static NativeBlip AddBlipWeapon(Vector3 target)
        {
            ADD_BLIP_FOR_WEAPON(target.X, target.Y, target.Z, out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }

        /// <summary>
        /// Tries to get the waypoint blip. Might actually not really be that efficient.
        /// </summary>
        /// <returns>If successful, the waypoint blip is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeBlip GetWaypoint()
        {
            for (int i = 0; i < 10; i++)
            {
                int handle = GET_FIRST_BLIP_INFO_ID(i);
                while (DOES_BLIP_EXIST(handle))
                {
                    if (GET_BLIP_SPRITE(handle) == (int)BlipIcon.Misc_Waypoint)
                        return new NativeBlip(handle);

                    handle = GET_NEXT_BLIP_INFO_ID(i);
                }
            }

            return null;
        }

        /// <summary>
        /// This adds a rectangle to the radar and map in the pause menu as seen in the TLAD Own The City multiplayer game mode.
        /// </summary>
        /// <param name="location">The start location of the blip.</param>
        /// <param name="size">The size of the blip.</param>
        /// <param name="color">The color of the blip.</param>
        /// <returns>If successful, the <see cref="NativeBlip"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeBlip AddBlipGangTerritory(Vector2 location, SizeF size, Color color)
        {
            ADD_BLIP_FOR_GANG_TERRITORY(location.X, location.Y, size.Width, size.Height, color.ToRgba(), out int handle);

            if (handle != 0)
                return new NativeBlip(handle);

            return null;
        }
        #endregion
    }
}

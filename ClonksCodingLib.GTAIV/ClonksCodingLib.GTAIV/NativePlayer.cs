using System;
using System.Drawing;
using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve players.
    /// </summary>
    public class NativePlayer
    {

        #region Variables and Properties

        // Variables
        private int pId;
        private NativePed pPed;
        private NativeGroup pGroup;

        // Properties
        public int ID
        {
            get
            {
                return pId;
            }
        }
        public int Index
        {
            get
            {
                return CONVERT_INT_TO_PLAYERINDEX((uint)ID);
            }
        }
        public int PedHandle
        {
            get
            {
                GET_PLAYER_CHAR(Index, out int ped);
                return ped;
            }
        }

        public NativePed Character
        {
            get
            {
                int handle = PedHandle;

                if (handle <= 0)
                {
                    return pPed != null ? pPed : null;
                }

                if (pPed != null)
                {
                    if (handle == pPed.Handle)
                        return pPed;
                    else
                        pPed.SetHandle(handle);
                }
                else
                {
                    pPed = new NativePed(handle);
                }

                return pPed;
            }
        }
        public NativeGroup Group
        {
            get
            {
                GET_PLAYER_GROUP(Index, out int group);

                if (group == 0)
                    return null;

                if (pGroup != null)
                {
                    if (group == pGroup.Handle)
                        return pGroup;
                    else
                        pPed.SetHandle(group);
                }
                else
                {
                    pGroup = new NativeGroup(group);
                }

                return pGroup;
            }
        }

        public string Name
        {
            get
            {
                return GET_PLAYER_NAME(Index);
            }
        }

        public NativeModel Model
        {
            get
            {
                GET_CHAR_MODEL(PedHandle, out int model);
                return new NativeModel(model);
            }
            // TODO: Add set
        }

        public Color Color
        {
            get
            {
                GET_PLAYER_RGB_COLOUR(Index, out int r, out int g, out int b);
                return Color.FromArgb(r, g, b);
            }
        }

        public bool IsActive
        {
            get
            {
                if (IsLocalPlayer)
                    return Character != null;

                return IS_NETWORK_PLAYER_ACTIVE(ID);
            }
        }
        public bool CanControlCharacter
        {
            get
            {
                return IS_PLAYER_CONTROL_ON(Index);
            }
            set
            {
                SET_PLAYER_CONTROL(Index, value);
            }
        }
        public bool IsLocalPlayer
        {
            get
            {
                return pId == (int)GET_PLAYER_ID();
            }
        }
        public bool IsOnMission
        {
            get
            {
                return !CAN_PLAYER_START_MISSION(Index);
            }
        }
        public bool IsPlaying
        {
            get
            {
                return IS_PLAYER_PLAYING(Index);
            }
        }
        public bool IsPressingHorn
        {
            get
            {
                return IS_PLAYER_PRESSING_HORN(Index);
            }
        }
        public bool IgnoredByEveryone
        {
            set
            {
                SET_EVERYONE_IGNORE_PLAYER(Index, value);
            }
        }
        public bool CanControlRagdoll
        {
            set
            {
                GIVE_PLAYER_RAGDOLL_CONTROL(Index, value);
            }
        }
        public bool NeverGetsTired
        {
            set
            {
                SET_PLAYER_NEVER_GETS_TIRED(Index, value);
            }
        }
        public int WantedLevel
        {
            get
            {
                STORE_WANTED_LEVEL(Index, out uint value);
                return (int)value;
            }
            set
            {
                if (value > 0)
                    ALTER_WANTED_LEVEL(Index, (uint)value);
                else
                    CLEAR_WANTED_LEVEL(Index);

                APPLY_WANTED_LEVEL_CHANGE_NOW(Index);
            }
        }
        public int Money
        {
            get
            {
                STORE_SCORE(Index, out uint value);
                return (int)value;
            }
            set
            {
                STORE_SCORE(Index, out uint s);

                if (value < 0)
                    value = 0;

                ADD_SCORE(Index, value - (int)s);
            }
        }
        public int MaxHealth
        {
            set
            {
                INCREASE_PLAYER_MAX_HEALTH(Index, value + 100);
            }
        }
        public int MaxArmor
        {
            set
            {
                INCREASE_PLAYER_MAX_ARMOUR(Index, value);
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativePlayer"/> class.
        /// </summary>
        /// <param name="id">The ID of the player.</param>
        public NativePlayer(int id)
        {
            pId = id;
        }
        #endregion

        #region Functions
        public bool IsTargetting(NativePed ped)
        {
            if (ped == null)
                return false;
            if (!ped.Exists())
                return false;

            return IS_PLAYER_FREE_AIMING_AT_CHAR(Index, ped.Handle);
        }
        public NativePed GetTargetedPed()
        {
            IVPool pedPool = IVPools.GetPedPool();

            for (int i = 0; i < pedPool.Count; i++)
            {
                UIntPtr ptr = pedPool.Get(i);

                if (ptr == UIntPtr.Zero)
                    continue;

                int handle = (int)pedPool.GetIndex(ptr);

                if (IS_PLAYER_FREE_AIMING_AT_CHAR(Index, handle))
                    return new NativePed(handle);
            }

            return null;
        }
        #endregion

        #region Methods
        public void ActivateMultiplayerSkin()
        {
            if (!IsLocalPlayer)
                return;

            SET_PLAYERSETTINGS_MODEL_VARIATIONS_CHOICE(Index);
            Character.SetDefaultVoice();
        }

        public void TeleportTo(Vector3 Position)
        {
            if (!IsLocalPlayer)
                return;

            NativePed p = Character;
            p.Position = Position;
            NativeWorld.LoadEnvironmentNow(Position);

            if (Position.Z == 0.0f)
                p.Position = NativeWorld.GetGroundPosition(Position, GroundType.Highest);
        }
        public void TeleportTo(float X, float Y)
        {
            TeleportTo(new Vector3(X, Y, 0.0f));
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Gives you easy access to native functions that involve groups.
    /// </summary>
    public class NativeGroup : HandleObject
    {

        private const int MAX_GROUP_SIZE = 7;

        #region Properties
        /// <summary>
        /// Gets or sets the leader of this <see cref="NativeGroup"/>.
        /// </summary>
        public IVPed Leader
        {
            get
            {
                if (!Exists())
                    return null;

                GET_GROUP_LEADER(Handle, out int leaderHandle);

                if (leaderHandle <= 0)
                    return null;

                return NativeWorld.GetPedInstaceFromHandle(leaderHandle);
            }
            set
            {
                if (!Exists())
                    return;
                if (value == null)
                    return;
                if (!value.Exists())
                    return;

                SET_GROUP_LEADER(Handle, value.GetHandle());
            }
        }

        /// <summary>
        /// Gets how members this <see cref="NativeGroup"/> has.
        /// </summary>
        public int MemberCount
        {
            get
            {
                if (!Exists())
                    return 0;

                GET_GROUP_SIZE(Handle, out int startIndex, out int count);
                return count;
            }
        }
        /// <summary>
        /// Gets how members a <see cref="NativeGroup"/> can have in it.
        /// </summary>
        public static int MaxMemberCount
        {
            get
            {
                return MAX_GROUP_SIZE;
            }
        }

        /// <summary>
        /// Sets the separation range of the peds in this <see cref="NativeGroup"/>.
        /// </summary>
        public float SeparationRange
        {
            set
            {
                if (!Exists())
                    return;

                SET_GROUP_SEPARATION_RANGE(Handle, value);
            }
        }
        /// <summary>
        /// Sets the follow status of the peds in this <see cref="NativeGroup"/>.
        /// <para>
        /// Mostly 1 in sco scripts.
        /// </para>
        /// </summary>
        public int FollowStatus
        {
            set
            {
                if (!Exists())
                    return;

                SET_GROUP_FOLLOW_STATUS(Handle, value);
            }
        }

        // TODO: Check if this can go up to 4
        /// <summary>
        /// Gets or sets the formation of the peds in this <see cref="NativeGroup"/>.
        /// <para>
        /// Mostly 0 in sco scripts but can go up to 3.
        /// </para>
        /// </summary>
        public int Formation
        {
            get
            {
                if (!Exists())
                    return 0; // TODO: Check sco scripts for formation values

                GET_GROUP_FORMATION(Handle, out int formation);
                return formation;
            }
            set
            {
                if (!Exists())
                    return;

                SET_GROUP_FORMATION(Handle, value);
            }
        }
        /// <summary>
        /// Gets or sets the formating spacing of the peds in this <see cref="NativeGroup"/>.
        /// </summary>
        public float FormationSpacing
        {
            get
            {
                if (!Exists())
                    return 0f;

                GET_GROUP_FORMATION_SPACING(Handle, out float spacing);
                return spacing;
            }
            set
            {
                if (!Exists())
                    return;

                SET_GROUP_FORMATION_SPACING(Handle, value);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="NativeGroup"/> class with an existing handle.
        /// </summary>
        /// <param name="handle">The handle of an already existing group.</param>
        public NativeGroup(int handle) : base(handle)
        {
            
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_GROUP_EXIST(Handle);
        }

        /// <summary>
        /// Adds a <see cref="IVPed"/> to this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="IVPed"/> to add.</param>
        /// <param name="neverLeave">Sets if the <see cref="IVPed"/> should never leave the group.</param>
        /// <returns>True if the ped got added. Otherwise, false.</returns>
        public bool AddMember(IVPed ped, bool neverLeave)
        {
            if (!Exists())
                return false;
            if (ped == null)
                return false;
            if (!ped.Exists())
                return false;

            if (MemberCount >= MaxMemberCount)
                return false;

            int pedHandle = ped.GetHandle();

            SET_GROUP_MEMBER(Handle, pedHandle);

            if (neverLeave)
                SET_CHAR_NEVER_LEAVES_GROUP(pedHandle, true);

            return true;
        }
        /// <summary>
        /// Adds a ped to this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The ped to add.</param>
        /// <param name="neverLeave">Sets if the ped should never leave the group.</param>
        /// <returns>True if the ped got added. Otherwise, false.</returns>
        public bool AddMember(int ped, bool neverLeave)
        {
            if (ped <= 0)
                return false;

            return AddMember(NativeWorld.GetPedInstaceFromHandle(ped), neverLeave);
        }

        /// <summary>
        /// Adds a <see cref="IVPed"/> to this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="IVPed"/> to add.</param>
        /// <returns>True if the ped got added. Otherwise, false.</returns>
        public bool AddMember(IVPed ped)
        {
            return AddMember(ped, false);
        }
        /// <summary>
        /// Adds a ped to this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The ped to add.</param>
        /// <returns>True if the ped got added. Otherwise, false.</returns>
        public bool AddMember(int ped)
        {
            if (ped <= 0)
                return false;

            return AddMember(NativeWorld.GetPedInstaceFromHandle(ped));
        }

        /// <summary>
        /// Gets a group member by its index.
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns></returns>
        public IVPed GetMember(int index)
        {
            if (!Exists())
                return null;

            GET_GROUP_MEMBER(Handle, index, out int memberHandle);

            if (memberHandle == 0)
                return null;

            return NativeWorld.GetPedInstaceFromHandle(memberHandle);
        }

        /// <summary>
        /// Checks if the <see cref="IVPed"/> is the leader of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="IVPed"/> to check for.</param>
        /// <returns>True if this <see cref="IVPed"/> is the leader. Otherwise, false.</returns>
        public bool IsLeader(IVPed ped)
        {
            if (!Exists())
                return false;
            if (ped == null)
                return false;
            if (!ped.Exists())
                return false;

            return IS_GROUP_LEADER(ped.GetHandle(), Handle);
        }
        /// <summary>
        /// Checks if the ped is the leader of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The ped to check for.</param>
        /// <returns>True if this ped is the leader. Otherwise, false.</returns>
        public bool IsLeader(int ped)
        {
            if (ped <= 0)
                return false;

            return IsLeader(NativeWorld.GetPedInstaceFromHandle(ped));
        }

        /// <summary>
        /// Checks if the <see cref="IVPed"/> is a member of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="IVPed"/> to check for.</param>
        /// <returns>True if this <see cref="IVPed"/> is a member. Otherwise, false.</returns>
        public bool IsMember(IVPed ped)
        {
            if (!Exists())
                return false;
            if (ped == null)
                return false;
            if (!ped.Exists())
                return false;

            return IS_GROUP_MEMBER(ped.GetHandle(), Handle);
        }
        /// <summary>
        /// Checks if the ped is a member of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The ped to check for.</param>
        /// <returns>True if this ped is a member. Otherwise, false.</returns>
        public bool IsMember(int ped)
        {
            if (ped <= 0)
                return false;

            return IsMember(NativeWorld.GetPedInstaceFromHandle(ped));
        }

        /// <summary>
        /// Returns an array of every member of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="includeLeader">If this array should include the leader of this <see cref="NativeGroup"/>.</param>
        /// <returns>An array of every members of this <see cref="NativeGroup"/>. <see langword="null"/> if this <see cref="NativeGroup"/> does not exists anymore.</returns>
        public IVPed[] ToArray(bool includeLeader)
        {
            if (!Exists())
                return null;

            return ToList(includeLeader).ToArray();
        }
        /// <summary>
        /// Returns a list of every member of this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="includeLeader">If this list should include the leader of this <see cref="NativeGroup"/>.</param>
        /// <returns>A list of every members of this <see cref="NativeGroup"/>. <see langword="null"/> if this <see cref="NativeGroup"/> does not exists anymore.</returns>
        public List<IVPed> ToList(bool includeLeader)
        {
            if (!Exists())
                return null;

            List<IVPed> peds = new List<IVPed>();

            // Add leader to list
            if (includeLeader)
            {
                if (Leader != null)
                    peds.Add(Leader);
            }

            // Add all members to list
            for (int i = 0; i < MemberCount; i++)
            {
                IVPed ped = GetMember(i);

                if (ped != null)
                    peds.Add(ped);
            }

            return peds;
        }

        /// <summary>
        /// Creates a new <see cref="NativeGroup"/>.
        /// </summary>
        /// <returns>If successful, the newly created <see cref="NativeGroup"/> is returned. Otherwise, <see langword="null"/>.</returns>
        public static NativeGroup Create()
        {
            CREATE_GROUP(false, out int group, true);

            if (group <= 0)
                return null;
            
            return new NativeGroup(group);
        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Dispose()
        {
            if (Exists())
                REMOVE_GROUP(Handle);

            base.Dispose();
        }

        /// <summary>
        /// Removes a <see cref="IVPed"/> from this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="ped">The <see cref="IVPed"/> to remove.</param>
        public void RemoveMember(IVPed ped)
        {
            if (!Exists())
                return;
            if (ped == null)
                return;
            if (!ped.Exists())
                return;

            ped.LeaveGroup();
        }
        /// <summary>
        /// Removes a member by its index from this <see cref="NativeGroup"/>.
        /// </summary>
        /// <param name="index">The index from which to remove the member.</param>
        public void RemoveMember(int index)
        {
            if (!Exists())
                return;

            GET_GROUP_MEMBER(Handle, index, out int memberHandle);

            if (memberHandle == 0)
                return;

            REMOVE_CHAR_FROM_GROUP(memberHandle);
        }
        /// <summary>
        /// Removes all members from this <see cref="NativeGroup"/>.
        /// </summary>
        public void RemoveAllMembers()
        {
            if (!Exists())
                return;

            for (int i = MemberCount - 1; i >= 0; i--)
                RemoveMember(i);
        }

        /// <summary>
        /// Makes all the group members of this <see cref="NativeGroup"/> enter a <see cref="IVVehicle"/>.
        /// </summary>
        /// <param name="veh">The <see cref="IVVehicle"/> the members should enter.</param>
        /// <param name="withLeader">If the leader <see cref="IVPed"/> should enter too.</param>
        /// <param name="keepCurrentDriver">If the current driver <see cref="IVPed"/> of the <see cref="IVVehicle"/> should be keept.</param>
        public void EnterVehicle(IVVehicle veh, bool withLeader, bool keepCurrentDriver)
        {
            if (!Exists())
                return;
            if (veh == null)
                return;
            if (!veh.Exists())
                return;

            List<IVPed> peds = ToList(false);

            // Leader should also enter vehicle
            if (withLeader)
            {
                if (Leader != null)
                    peds.Insert(0, Leader);
            }

            if (peds.Count == 0)
                return;

            if (keepCurrentDriver && veh.IsSeatFree(VehicleSeat.Driver))
                keepCurrentDriver = false;

            int index = 0;

            if (!keepCurrentDriver)
            {
                _TASK_ENTER_CAR_AS_DRIVER(peds[index].GetHandle(), veh.GetHandle(), 0);
                index++;
            }

            int seats = veh.GetMaximumNumberOfPassengers();
            for (int seat = 0; seat < seats; seat++)
            {
                if (index >= peds.Count)
                    return;

                _TASK_ENTER_CAR_AS_PASSENGER(peds[index].GetHandle(), veh.GetHandle(), 0, (uint)seat);
                index++;
            }
        }
        /// <summary>
        /// Makes all the group members of this <see cref="NativeGroup"/> enter a <see cref="IVVehicle"/>.
        /// </summary>
        /// <param name="veh">The vehicle the members should enter.</param>
        /// <param name="withLeader">If the leader <see cref="IVPed"/> should enter too.</param>
        /// <param name="keepCurrentDriver">If the current driver <see cref="IVPed"/> of the <see cref="IVVehicle"/> should be keept.</param>
        public void EnterVehicle(int veh, bool withLeader, bool keepCurrentDriver)
        {
            if (veh <= 0)
                return;

            EnterVehicle(NativeWorld.GetVehicleInstaceFromHandle(veh), withLeader, keepCurrentDriver);
        }
        #endregion

    }
}

using System;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativeRoom
    {

        #region Variables and Properties
        // Variables
        private int _room;
        private int _interior;

        // Properties
        public int Room
        {
            get { return _room; }
            private set { _room = value; }
        }
        public int Interior
        {
            get { return _interior; }
            private set { _interior = value; }
        }
        #endregion

        #region Constructor
        internal NativeRoom(int room, int interior)
        {
            Room = room;
            Interior = interior;
        }
        #endregion

        #region Functions
        // Statics
        public static NativeRoom FromString(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            string[] arr = name.Trim().Split('_');
            if (arr.Length != 3 || arr[0] != "R")
                return null;

            return new NativeRoom(Convert.ToInt32(arr[1], 16), Convert.ToInt32(arr[2], 16));
        }

        public static NativeRoom FromPed(CPed ped)
        {
            if (ped == null)
                return null;

            UIntPtr ptr = ped.GetUIntPtr();
            if (ptr == UIntPtr.Zero)
                return null;

            int pedHandle = (int)CPools.GetPedPool().GetIndex(ptr);
            if (pedHandle == 0)
                return null;

            GET_KEY_FOR_CHAR_IN_ROOM(pedHandle, out uint room);
            GET_INTERIOR_FROM_CHAR(pedHandle, out int interior);

            return new NativeRoom((int)room, interior);
        }
        public static NativeRoom FromPed(int pedHandle)
        {
            if (pedHandle == 0)
                return null;

            GET_KEY_FOR_CHAR_IN_ROOM(pedHandle, out uint room);
            GET_INTERIOR_FROM_CHAR(pedHandle, out int interior);

            return new NativeRoom((int)room, interior);
        }

        public static NativeRoom FromVehicle(CVehicle vehicle)
        {
            if (vehicle == null)
                return null;

            UIntPtr ptr = vehicle.GetUIntPtr();
            if (ptr == UIntPtr.Zero)
                return null;

            int vehicleHandle = (int)CPools.GetVehiclePool().GetIndex(ptr);
            if (vehicleHandle == 0)
                return null;

            GET_KEY_FOR_CAR_IN_ROOM(vehicleHandle, out uint room);
            GET_INTERIOR_FROM_CAR(vehicleHandle, out int interior);

            return new NativeRoom((int)room, interior);
        }
        public static NativeRoom FromVehicle(int vehicleHandle)
        {
            if (vehicleHandle == 0)
                return null;

            GET_KEY_FOR_CAR_IN_ROOM(vehicleHandle, out uint room);
            GET_INTERIOR_FROM_CAR(vehicleHandle, out int interior);

            return new NativeRoom((int)room, interior);
        }
        #endregion

        public override string ToString()
        {
            return string.Format("R_{0}_{1}", Room.ToString("X" + 8), Interior.ToString("X" + 8));
        }

    }
}

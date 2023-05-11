using System.Numerics;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    public class NativeFire : HandleObject
    {

        // START_OBJECT_FIRE, EXTINGUISH_OBJECT_FIRE

        #region Properties
        public Vector3 Position
        {
            get {
                if (!IsValid)
                    return Vector3.Zero;

                GET_SCRIPT_FIRE_COORDS(Handle, out Vector3 pos);
                return pos;
            }
        }
        #endregion

        #region Constructor
        internal NativeFire(int handle) : base(handle)
        {

        }
        #endregion

        #region Methods
        /// <inheritdoc/>
        public override void Dispose()
        {
            if (Exists())
                REMOVE_SCRIPT_FIRE(Handle);

            base.Dispose();
        }

        // Statics
        public static void ExtinguishFireAtPoint(Vector3 pos, float radius)
        {
            EXTINGUISH_FIRE_AT_POINT(pos.X, pos.Y, pos.Z, radius);
        }
        public static void ExtinguishCarFire(CVehicle veh)
        {
            if (veh != null)
                EXTINGUISH_CAR_FIRE(veh.GetHandle());
        }
        public static void ExtinguishCharFire(CPed ped)
        {
            if (ped != null)
                EXTINGUISH_CHAR_FIRE(ped.GetHandle());
        }
        #endregion

        #region Functions
        /// <inheritdoc/>
        public override bool Exists()
        {
            if (!IsValid)
                return false;

            return DOES_SCRIPT_FIRE_EXIST(Handle);
        }

        /// <summary>
        /// Checks if this <see cref="NativeFire"/> is extinguished.
        /// </summary>
        /// <returns>True if extinguished. Otherwise, false.</returns>
        public bool IsFireExtinguished()
        {
            if (!Exists())
                return true;

            return IS_SCRIPT_FIRE_EXTINGUISHED(Handle);
        }

        // Statics
        /// <summary>
        /// Gets the number of fires that are within this <paramref name="range"/> at the given <paramref name="pos"/>.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="range">The range.</param>
        /// <returns>The number of fires.</returns>
        public static uint GetNumberOfFiresInRange(Vector3 pos, float range)
        {
            return GET_NUMBER_OF_FIRES_IN_RANGE(pos, range);
        }
        /// <summary>
        /// Gets the number of fires that are within this area.
        /// </summary>
        /// <param name="pos1">Position 1</param>
        /// <param name="pos2">Position 2</param>
        /// <returns>The number of fires.</returns>
        public static uint GetNumberOfFiresInArea(Vector3 pos1, Vector3 pos2)
        {
            return GET_NUMBER_OF_FIRES_IN_AREA(pos1, pos2);
        }

        /// <summary>
        /// Starts a new fire at the given <paramref name="position"/>.
        /// </summary>
        /// <param name="position">The position of the fire.</param>
        /// <param name="numGenerationsAllowed">Undocumented. Usually 5.</param>
        /// <param name="strength">The strength of the fire. Usually 3.</param>
        /// <returns>If successful, the newly created fire is returned. Otherwise, false.</returns>
        public static NativeFire StartFire(Vector3 position, uint numGenerationsAllowed, uint strength)
        {
            int handle = START_SCRIPT_FIRE(position, numGenerationsAllowed, strength);
            
            if (handle == 0)
                return null;
            
            return new NativeFire(handle);
        }

        /// <summary>
        /// Starts a new char fire for the specified <paramref name="ped"/>.
        /// </summary>
        /// <param name="ped">The <see cref="CPed"/> who should be set on fire.</param>
        /// <returns>If successful, the newly created fire is returned. Otherwise, false.</returns>
        public static NativeFire StartCharFire(CPed ped)
        {
            if (ped == null)
                return null;

            int handle = START_CHAR_FIRE(ped.GetHandle());

            if (handle == 0)
                return null;

            return new NativeFire(handle);
        }

        /// <summary>
        /// Starts a new car fire for the specified <paramref name="veh"/>.
        /// </summary>
        /// <param name="veh">The <see cref="CVehicle"/> who should be set on fire.</param>
        /// <returns>If successful, the newly created fire is returned. Otherwise, false.</returns>
        public static NativeFire StartCarFire(CVehicle veh)
        {
            if (veh == null)
                return null;

            int handle = START_CAR_FIRE(veh.GetHandle());

            if (handle == 0)
                return null;

            return new NativeFire(handle);
        }
        #endregion

    }
}

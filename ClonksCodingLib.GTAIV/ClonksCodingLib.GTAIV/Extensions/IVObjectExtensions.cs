using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using IVSDKDotNet;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{
    /// <summary>
    /// Contains extensions for the <see cref="IVObject"/> class.
    /// </summary>
    public static class IVObjectExtensions
    {
        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="CObject"/>.
        /// <para>Can be used for calling native functions like <see cref="DOES_OBJECT_EXIST(int)"/> which requires a ped handle.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>If successful, the handle of this <see cref="CObject"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this IVObject obj)
        {
            if (obj == null)
                return 0;

            return (int)IVPools.GetObjectPool().GetIndex(obj.GetUIntPtr());
        }

        public static bool IsPlayerTargettingObject(this IVObject obj, int playerIndex)
        {
            if (obj == null)
                return false;

            return IS_PLAYER_TARGETTING_OBJECT(playerIndex, GetHandle(obj));
        }
        #endregion

        #region Methods
        public static void ApplyForce(this IVObject obj, Vector3 direction, Vector3 rotation)
        {
            if (obj == null)
                return;

            APPLY_FORCE_TO_OBJECT(GetHandle(obj), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 1, 1);
        }
        public static void ApplyForce(this IVObject obj, Vector3 direction)
        {
            if (obj == null)
                return;

            ApplyForce(obj, direction, Vector3.Zero);
        }

        public static void ApplyForceRelative(this IVObject obj, Vector3 direction, Vector3 rotation)
        {
            if (obj == null)
                return;

            APPLY_FORCE_TO_OBJECT(GetHandle(obj), 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, 0, 1, 1, 1);
        }
        public static void ApplyForceRelative(this IVObject obj, Vector3 direction)
        {
            if (obj == null)
                return;

            ApplyForceRelative(obj, direction, Vector3.Zero);
        }

        public static void MakeObjectTargetable(this IVObject obj, bool targetable)
        {
            if (obj == null)
                return;

            MAKE_OBJECT_TARGETTABLE(GetHandle(obj), targetable);
        }

        public static void ClearObjectLastDamageEntity(this IVObject obj)
        {
            if (obj == null)
                return;

            CLEAR_OBJECT_LAST_DAMAGE_ENTITY(GetHandle(obj));
        }
        #endregion
    }
}

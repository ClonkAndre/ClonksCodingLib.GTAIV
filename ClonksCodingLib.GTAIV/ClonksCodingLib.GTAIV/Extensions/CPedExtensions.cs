using System;
using System.Numerics;

using IVSDKDotNet;
using IVSDKDotNet.Enums;
using static IVSDKDotNet.Native.Natives;

namespace CCL.GTAIV
{

    /// <summary>
    /// Contains ped genders.
    /// </summary>
    public enum PedGender
    {
        /// <summary>
        /// Unknown gender. You will only see this appearing when for example the <see cref="CPed"/> is <see langword="null"/>.
        /// </summary>
        Unkown,
        /// <summary>
        /// Ped is a male.
        /// </summary>
        Male,
        /// <summary>
        /// Ped is a female.
        /// </summary>
        Female
    }

    /// <summary>
    /// Contains extensions for the <see cref="CPed"/> class.
    /// </summary>
    public static class CPedExtensions
    {
        #region Methods
        /// <summary>
        /// Adds ammo to the given <paramref name="weapon"/> for this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="weapon">The weapon to fill up the ammo.</param>
        /// <param name="amount">The amount of ammo to fill up.</param>
        public static void AddAmmoToChar(this CPed ped, eWeaponType weapon, uint amount)
        {
            if (ped == null)
                return;

            ADD_AMMO_TO_CHAR(GetHandle(ped), (uint)weapon, amount);
        }

        /// <summary>
        /// Adds armour to this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="amount">The amount of armour to fill up.</param>
        public static void AddArmourToChar(this CPed ped, uint amount)
        {
            if (ped == null)
                return;

            ADD_ARMOUR_TO_CHAR(GetHandle(ped), amount);
        }

        /// <summary>
        /// Sets if this <see cref="CPed"/> can ragdoll.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="value">Can ragdoll or not.</param>
        public static void PreventRagdoll(this CPed ped, bool value)
        {
            if (ped == null)
                return;

            UNLOCK_RAGDOLL(GetHandle(ped), !value);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Gets the handle for this <see cref="CPed"/>.
        /// <para>Can be used for calling native functions like <see cref="EXPLODE_CHAR_HEAD(int)"/> which requires a ped handle.</para>
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>If successful, the handle of this <see cref="CPed"/> is returned. Otherwise, 0.</returns>
        public static int GetHandle(this CPed ped)
        {
            if (ped == null)
                return 0;

            return (int)CPools.GetPedPool().GetIndex(ped.GetUIntPtr());
        }

        /// <summary>
        /// Gets the <see cref="Rectangle3D"/> (or bounds) of this <see cref="CPed"/> with their current model.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>If successful, the <see cref="Rectangle3D"/> of this <see cref="CPed"/> is returned. Otherwise, <see cref="Rectangle3D.Empty()"/> is returned.</returns>
        public static Rectangle3D GetModelRect3D(this CPed ped)
        {
            if (ped == null)
                return Rectangle3D.Empty();

            int handle = GetHandle(ped);
            GET_CHAR_MODEL(handle, out uint pModel);

            if (pModel == 0)
                return Rectangle3D.Empty();

            GET_MODEL_DIMENSIONS(pModel, out Vector3 min, out Vector3 max);
            return new Rectangle3D(ped.Matrix.pos, max);
        }

        /// <summary>
        /// Gets if there are any chars near this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <param name="radius">The radius in which to search for.</param>
        /// <returns>True if there are any chars in the area around this <see cref="CPed"/>. Otherwise, false.</returns>
        public static bool AreAnyCharsNearChar(this CPed ped, float radius)
        {
            if (ped == null)
                return false;

            return ARE_ANY_CHARS_NEAR_CHAR(GetHandle(ped), radius);
        }

        /// <summary>
        /// Gets the model of this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>The model of this <see cref="CPed"/>.</returns>
        public static uint GetCharModel(this CPed ped)
        {
            if (ped == null)
                return 0;

            GET_CHAR_MODEL(GetHandle(ped), out uint pModel);
            return pModel;
        }

        /// <summary>
        /// Gets the gender of this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>One of the 3 items in the <see cref="PedGender"/> enum.</returns>
        public static PedGender GetCharGender(this CPed ped)
        {
            if (ped == null)
                return PedGender.Unkown;

            return IS_CHAR_MALE(GetHandle(ped)) ? PedGender.Male : PedGender.Female;
        }

        /// <summary>
        /// Gets the height above ground of this <see cref="CPed"/>.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns>The height above ground value.</returns>
        public static float GetHeightAboveGround(this CPed ped)
        {
            if (ped == null)
                return 0f;

            GET_CHAR_HEIGHT_ABOVE_GROUND(GetHandle(ped), out float val);
            return val;
        }
        #endregion
    }

}
